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

namespace BetterDocs.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;

        public DocumentService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
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

            return _dbContext.TextDocuments
                .Where(document =>
                    document.Owner.Id.Equals(user.Id))
                .ToList();
        }

        public List<TextDocument> GetDocumentsSharedWithUser()
        {
            var user = GetApplicationUser();

            return _dbContext.TextDocuments
                .Where(document => document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)))
                .ToList();
        }

        public TextDocument CreateDocument(TextDocumentModel textDocument)
        {
            var document = new TextDocument
                {Text = textDocument.Text ?? "", Name = textDocument.Name, Owner = GetApplicationUser()};

            var entityEntry = _dbContext.TextDocuments.Add(document);
            _dbContext.SaveChanges();

            return entityEntry.Entity;
        }

        public TextDocument GetDocument(string id)
        {
            var user = GetApplicationUser();

            TextDocument firstOrDefault = _dbContext.TextDocuments
                .Where(document => document.Id.Equals(id))
                .FirstOrDefault(document =>
                    document.Owner.Id.Equals(user.Id) || document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)));
            return firstOrDefault;
        }

        public TextDocument GetDocumentWithoutCheckingAccess(string id)
        {
            return _dbContext.TextDocuments
                .FirstOrDefault(document => document.Id.Equals(id));
        }

        public void RemoveDocument(string id)
        {
            var user = GetApplicationUser();

            var textDocument = _dbContext.TextDocuments
                .Where(document => document.Id.Equals(id))
                .FirstOrDefault(document => document.Owner.Id.Equals(user.Id));

            if (textDocument == null)
            {
                return;
            }

            _dbContext.TextDocuments.Remove(textDocument);
            _dbContext.SaveChanges();
            ClearCache(id);
        }

        public TextDocument UpdateDocument(string text, string documentId)
        {
            var user = GetApplicationUser();

            var textDocument = _dbContext.TextDocuments
                .Where(document =>
                    document.Owner.Id.Equals(user.Id) || document.DocumentsSharing.Any(u => u.UserId.Equals(user.Id)))
                .FirstOrDefault(document => document.Id.Equals(documentId));

            if (textDocument == null)
            {
                return null;
            }

            textDocument.Text = text;
            _dbContext.TextDocuments.Update(textDocument);
            _dbContext.SaveChanges();

            ClearCache(documentId);
            return textDocument;
        }

        private ApplicationUser GetApplicationUser()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
        }

        public void AddContributor(string documentId, string email)
        {
            var textDocument = _dbContext.TextDocuments.Find(documentId);

            if (textDocument == null || !CanEditDocument(textDocument)) return;

            var contributor = _dbContext.ApplicationUsers.FirstOrDefault(user => user.Email.Equals(email));

            if (contributor == null) return;

            textDocument.DocumentsSharing.Add(new ShareDocument {UserId = contributor.Id, DocumentId = textDocument.Id});
            _dbContext.TextDocuments.Update(textDocument);
            _dbContext.SaveChanges();

            ClearCache(documentId);
        }

        public void RemoveContributor(string documentId, string email)
        {
            var textDocument = _dbContext.TextDocuments.Find(documentId);

            if (textDocument == null || !CanEditDocument(textDocument)) return;

            var contributor = _dbContext.ApplicationUsers.FirstOrDefault(user => user.Email.Equals(email));

            if (contributor == null) return;

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
            _distributedCache.Remove("documents/" + documentId + "/text");
            _distributedCache.Remove("documents/" + documentId + "/users");
        }

        private ApplicationUser GetApplicationUser()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
        }
    }
}