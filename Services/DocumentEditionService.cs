using System;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Data;
using BetterDocs.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BetterDocs.Services
{
    public class DocumentEditionService
    {
        private readonly DocumentsDbContext _documentsContext;

        public DocumentEditionService(DbContextOptions<DocumentsDbContext> dbOptions)
        {
            _documentsContext = new DocumentsDbContext(dbOptions);
        }

        public TextDocument UpdateDocument(ApplicationUser user, string text, string documentId)
        {
            var textDocument = _documentsContext.TextDocuments
                // TODO: Check access to the document
                //.Where(document => document.ContributorId.Equals(user.Id))
                .FirstOrDefault(document => document.Id.Equals(documentId));

            if (textDocument == null)
            {
                // TODO: Return that the document doesn't exist or the user has no permission to view it.
                return null;
            }

            // TODO: don't append text, merge it somehow
            textDocument.Text += text;
            _documentsContext.TextDocuments.Update(textDocument);

            return textDocument;
        }

        public TextDocument GetDocument(ApplicationUser user, string documentId)
        {
            return _documentsContext.TextDocuments
                //TODO: Check access to the document
                //.Where(document => document.ContributorId.Equals(user.Id))
                .FirstOrDefault(document => document.Id.Equals(documentId));
        }
    }
}