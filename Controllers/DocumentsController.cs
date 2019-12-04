using System.Collections.Generic;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetterDocs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentService _documentService;

        public DocumentsController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public List<TextDocument> GetTextDocuments()
        {
            return _documentService.GetDocumentsForUser();
        }

        [HttpGet("{id}")]
        public TextDocument GetTextDocument(string id)
        {
            return _documentService.GetDocument(id);
        }
        
        [HttpPost]
        public CreatedAtActionResult CreateTextDocument(TextDocumentModel textDocument)
        {
            var document = _documentService.CreateDocument(textDocument);
            return CreatedAtAction(nameof(GetTextDocument), new {id = document.Id}, document);
        }
    }
}