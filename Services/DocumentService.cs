using System.Collections.Generic;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Data;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BetterDocs.Services
{
    public class DocumentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<TextDocument> GetDocumentsForUser()
        {
            var user = GetApplicationUser();
            
            return _dbContext.TextDocuments
                .Where(document => document.Owner.Id.Equals(user.Id) || document.SharedWith.Any(u => u.Id.Equals(user.Id)))
                .ToList();
        }

        public TextDocument CreateDocument(TextDocumentModel textDocument)
        {
            var document = new TextDocument
                {Text = textDocument.Text, Name = textDocument.Name, Owner = GetApplicationUser()};

            var entityEntry = _dbContext.TextDocuments.Add(document);
            _dbContext.SaveChanges();

            return entityEntry.Entity;
        }

        public TextDocument GetDocument(string id)
        {
            var user = GetApplicationUser();
            
            return _dbContext.TextDocuments
                .Where(document => document.Id.Equals(id))
                .FirstOrDefault(document => document.Owner.Id.Equals(user.Id) || document.SharedWith.Any(u => u.Id.Equals(user.Id)));
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
        }

        public TextDocument UpdateDocument(string text, string documentId)
        {
            var user = GetApplicationUser();
            
            var textDocument = _dbContext.TextDocuments
                .Where(document => document.Owner.Id.Equals(user.Id) || document.SharedWith.Any(u => u.Id.Equals(user.Id)))
                .FirstOrDefault(document => document.Id.Equals(documentId));

            if (textDocument == null)
            {
                // TODO: Return that the document doesn't exist or the user has no permission to view it.
                return null;
            }
            
            textDocument.Text = text;
            _dbContext.TextDocuments.Update(textDocument);

            return textDocument;
        }

        private ApplicationUser GetApplicationUser()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
        }
    }
}