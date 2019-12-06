using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Data;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<IDocumentService> _logger;

        public DocumentService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, ILogger<IDocumentService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public List<TextDocument> GetDocumentsForUser()
        {
            var user = GetApplicationUser();

            return _dbContext.TextDocuments
                .Where(document =>
                    document.Owner.Id.Equals(user.Id) || document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)))
                .ToList();
        }

        public List<TextDocument> GetDocumentsCreatedByUser()
        {
            var user = GetApplicationUser();
            _logger.LogInformation("Fetching documents created by user {}", user.UserName);

            return _dbContext.TextDocuments
                .Where(document =>
                    document.Owner.Id.Equals(user.Id))
                .ToList();
        }

        public List<TextDocument> GetDocumentsSharedWithUser()
        {
            var user = GetApplicationUser();

            _logger.LogInformation("Fetching documents shared with user {}", user.UserName);
            var documentsSharedWithUser = _dbContext.TextDocuments
                .Where(document => document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)))
                .ToList();
            RefreshDbContext();

            return documentsSharedWithUser;
        }

        public TextDocument CreateDocument(TextDocumentModel textDocument)
        {
            var document = new TextDocument
                {Text = textDocument.Text ?? "", Name = textDocument.Name, Owner = GetApplicationUser()};

            var entityEntry = _dbContext.TextDocuments.Add(document);
            _dbContext.SaveChanges();

            _logger.LogInformation("Document with Id {} created.", document.Id);
            return entityEntry.Entity;
        }

        public TextDocument GetDocument(string id)
        {
            var user = GetApplicationUser();

            var textDocument = _dbContext.TextDocuments
                .Where(document => document.Id.Equals(id))
                .FirstOrDefault(document =>
                    document.Owner.Id.Equals(user.Id) || document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)));
            RefreshDbContext();

            return textDocument;
        }

        public TextDocument GetDocumentWithoutCheckingAccess(string id)
        {
            var textDocument = _dbContext.TextDocuments
                .FirstOrDefault(document => document.Id.Equals(id));
            
            RefreshDbContext();
            return textDocument;
        }

        public void RemoveDocument(string id)
        {
            var user = GetApplicationUser();

            var textDocument = _dbContext.TextDocuments
                .Where(document => document.Id.Equals(id))
                .FirstOrDefault(document => document.Owner.Id.Equals(user.Id));

            if (textDocument == null)
            {
                _logger.LogWarning("Couldn't find document with Id {}", id);
                return;
            }

            _dbContext.TextDocuments.Remove(textDocument);
            _dbContext.SaveChanges();
            
            _logger.LogInformation("Document {} removed", id);
            ClearCache(id);
        }

        public TextDocument UpdateDocument(string text, string documentId)
        {
            var user = GetApplicationUser();

            _logger.LogInformation("Updating the document with ID {} by a user {}", documentId, user.UserName);
            var textDocument = _dbContext.TextDocuments
                .Where(document =>
                    document.Owner.Id.Equals(user.Id) || document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)))
                .FirstOrDefault(document => document.Id.Equals(documentId));

            if (textDocument == null)
            {
                _logger.LogWarning("Document not found, returning");
                return null;
            }

            RefreshDbContext();

            textDocument.Text = text;
            _dbContext.TextDocuments.Update(textDocument);
            _dbContext.SaveChanges();

            _logger.LogInformation("Document with ID {} updated", textDocument.Id);
            ClearCache(documentId);
            return textDocument;
        }

        public void AddContributor(string documentId, string email)
        {
            var textDocument = _dbContext.TextDocuments.Find(documentId);

            if (textDocument == null || !CanEditDocument(textDocument))
            {
                _logger.LogWarning(
                    "Cannot add a contributor to document with ID {}.\n Couldn't find this document or a user {} has no permission to view it",
                    documentId, email);
                return;
            }

            var contributor = _dbContext.ApplicationUsers.FirstOrDefault(user => user.Email.Equals(email));

            if (contributor == null)
            {
                _logger.LogWarning("Couldn't find user with email {}", email);
                return;
            }

            RefreshDbContext();

            textDocument.DocumentsSharing.Add(new ShareDocument
                {UserId = contributor.Id, DocumentId = textDocument.Id});
            _dbContext.TextDocuments.Update(textDocument);
            _dbContext.SaveChanges();
            
            _logger.LogInformation("User {} added as a contributor to the document {} successfully", email, documentId);
            ClearCache(documentId);
        }

        public void RemoveContributor(string documentId, string email)
        {
            var textDocument = _dbContext.TextDocuments.Find(documentId);

            if (textDocument == null || !CanEditDocument(textDocument)) return;

            var contributor = _dbContext.ApplicationUsers.FirstOrDefault(user => user.Email.Equals(email));

            if (contributor == null) return;

            RefreshDbContext();
            var documentSharing = textDocument.DocumentsSharing.First(x => x.UserId.Equals(contributor.Id));

            textDocument.DocumentsSharing.Remove(documentSharing);
            _dbContext.TextDocuments.Update(textDocument);
            _dbContext.SaveChanges();

            ClearCache(documentId);
        }

        private bool CanEditDocument([NotNull] TextDocument textDocument)
        {
            var userId = GetApplicationUser().Id;

            return textDocument.Owner.Id.Equals(userId) ||
                   textDocument.DocumentsSharing.Any(
                       contributor => contributor.UserId.Equals(userId)
                   );
        }

        private void ClearCache(string documentId)
        {
            _logger.LogInformation("Clearing cache");
            _distributedCache.Remove("documents/" + documentId + "/text");
            _distributedCache.Remove("documents/" + documentId + "/users");
        }

        private ApplicationUser GetApplicationUser()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
        }

        private void RefreshDbContext()
        {
            _logger.LogInformation("Refreshing DbContexts");
            _dbContext.ShareDocuments.ToList();
            _dbContext.ApplicationUsers.ToList();
        }
    }
}