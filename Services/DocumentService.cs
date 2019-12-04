using System.Collections.Generic;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Data;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BetterDocs.Services
{
    public class DocumentService
    {
        private readonly DocumentsDbContext _documentsContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentService(DocumentsDbContext documentsContext, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _documentsContext = documentsContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<TextDocument> GetDocumentsForUser()
        {
            return _documentsContext.TextDocuments
                .Where(document => document.OwnerId.Equals(GetApplicationUser().Id))
                // TODO: Check collaborators 
                .ToList();
        }

        public TextDocument CreateDocument(TextDocumentModel textDocument)
        {
            var document = new TextDocument
                {Text = textDocument.Text, Name = textDocument.Name, OwnerId = GetApplicationUser().Id};

            var entityEntry = _documentsContext.TextDocuments.Add(document);
            _documentsContext.SaveChanges();

            return entityEntry.Entity;
        }

        public TextDocument GetDocument(string id)
        {
            return _documentsContext.TextDocuments
                .Where(document => document.Id.Equals(id))
                .FirstOrDefault(document => document.OwnerId.Equals(GetApplicationUser().Id));
        }

        public TextDocument UpdateDocument(TextDocumentModel textDocumentModel)
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

            var textDocument = _documentsContext.TextDocuments
                .Where(document => document.ContributorId.Equals(user.Id))
                .FirstOrDefault(document => document.Id.Equals(textDocumentModel.Id));

            if (textDocument == null)
            {
                // TODO: Return that the document doesn't exist or the user has no permission to view it.
                return null;
            }

            _documentsContext.TextDocuments.Update(textDocument);
            _documentsContext.SaveChanges();

            return textDocument;
        }

        private ApplicationUser GetApplicationUser()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
        }

        public void RemoveDocument(string id)
        {
            var textDocument = _documentsContext.TextDocuments.Where(document => document.Id.Equals(id))
                .FirstOrDefault(document => document.OwnerId.Equals(GetApplicationUser().Id));

            if (textDocument == null) return;
            _documentsContext.TextDocuments.Remove(textDocument);
            _documentsContext.SaveChanges();
        }
    }
}