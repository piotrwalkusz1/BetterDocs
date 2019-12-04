using BetterDocs.Models;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetterDocs.Pages.Documents
{
    public class Create : PageModel
    {
        private readonly DocumentService _documentService;

        public Create(DocumentService documentService)
        {
            _documentService = documentService;
        }

        [BindProperty] public TextDocumentModel TextDocumentModel { get; set; }

        public IActionResult OnPost()
        {
            var textDocument = _documentService.CreateDocument(TextDocumentModel);
            return RedirectToPage("/EditDocument", new {id = textDocument.Id});
        }
    }
}