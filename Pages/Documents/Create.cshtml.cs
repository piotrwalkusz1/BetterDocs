using BetterDocs.Models;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetterDocs.Pages.Documents
{
    public class Create : PageModel
    {
        private readonly IDocumentService _documentService;

        public Create(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [BindProperty] public TextDocumentModel TextDocumentModel { get; set; }

        public IActionResult OnPost()
        {
            var textDocument = _documentService.CreateDocument(TextDocumentModel);
            return RedirectToPage("EditDocument", new {id = textDocument.Id});
        }
    }
}