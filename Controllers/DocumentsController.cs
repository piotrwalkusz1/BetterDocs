using System.Collections.Generic;
using BetterDocs.Data.Entities;
using BetterDocs.Filters;
using BetterDocs.Models;
using BetterDocs.Services;
using IronPdf;
using Microsoft.AspNetCore.Mvc;

namespace BetterDocs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public List<TextDocument> GetTextDocuments()
        {
            return _documentService.GetDocumentsForUser();
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(CallCounterFilter))]
        public TextDocument GetTextDocument(string id)
        {
            return _documentService.GetDocument(id);
        }

        [HttpPost]
        [ServiceFilter(typeof(CallCounterFilter))]
        public CreatedAtActionResult CreateTextDocument(TextDocumentModel textDocument)
        {
            var document = _documentService.CreateDocument(textDocument);
            return CreatedAtAction(nameof(GetTextDocument), new {id = document.Id}, document);
        }

        [HttpGet("{id}/pdf")]
        public IActionResult GetDocumentAsPdf(string id)
        {
            var textDocument = _documentService.GetDocument(id);

            var content = HtmlToPdf.StaticRenderHtmlAsPdf(textDocument.Text).Stream;
            var contentType = "APPLICATION/octet-stream";
            var fileName = textDocument.Name + ".pdf";

            return File(content, contentType, fileName);
        }
    }
}