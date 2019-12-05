using System.Collections.Generic;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Data;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace BetterDocs.Services
{
    public interface IDocumentService
    {
        List<TextDocument> GetDocumentsForUser();

        TextDocument CreateDocument(TextDocumentModel textDocument);

        TextDocument GetDocument(string id);

        TextDocument GetDocumentWithoutCheckingAccess(string id);

        void RemoveDocument(string id);

        TextDocument UpdateDocument(string text, string documentId);

        void AddContributor(string documentId, string email);

        void RemoveContributor(string documentId, string email);
    }
}