using System.Collections.Generic;
using System.Linq;
using BetterDocs.Models;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetterDocs.Pages.Documents
{
    public class List : PageModel
    {
        private readonly IDocumentService _documentService;

        public List(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public IList<TextDocumentModel> TextDocumentModels { get; set; }

        public void OnGet()
        {
            TextDocumentModels = _documentService.GetDocumentsForUser().Select(document =>
                    new TextDocumentModel {Name = document.Name, Text = document.Text, Id = document.Id})
                .ToList();
        }

        public IActionResult OnPostRemove(string id)
        {
            _documentService.RemoveDocument(id);
            return RedirectToPage("./Index");
        }
    }
}