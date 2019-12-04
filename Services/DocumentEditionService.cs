using System;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Data;
using BetterDocs.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BetterDocs.Services
{
    public class DocumentEditionService
    {
        private readonly DocumentsDbContext _documentsContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentEditionService(DbContextOptions<DocumentsDbContext> dbOptions, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _documentsContext = new DocumentsDbContext(dbOptions);
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public TextDocument UpdateDocument(string text, string documentId)
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
            
            var textDocument = _documentsContext.TextDocuments
                .Where(document => document.ContributorId.Equals(user.Id))
                .FirstOrDefault(document => document.Id.Equals(documentId));

            if (textDocument == null)
            {
                // TODO: Return that the document doesn't exist or the user has no permission to view it.
                return null;
            }

            // TODO: don't append text, merge it somehow
            textDocument.Text = text;
            _documentsContext.TextDocuments.Update(textDocument);

            return textDocument;
        }

        public TextDocument GetDocument(ApplicationUser user, string documentId)
        {
            return _documentsContext.TextDocuments
                .Where(document => document.ContributorId.Equals(user.Id))
                .FirstOrDefault(document => document.Id.Equals(documentId));
        }
    }
}