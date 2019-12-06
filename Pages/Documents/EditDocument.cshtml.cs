using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BetterDocs.Areas.Identity;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Pages
{
    public class EditDocumentModel : PageModel
    {
        private readonly ILogger<EditDocumentModel> _logger;
        public readonly IDocumentService _documentService;

        [FromQuery(Name = "id")] public string DocumentId { get; set; }
        public ICollection<ApplicationUser> Contributors { get; set; }

        public EditDocumentModel(ILogger<EditDocumentModel> logger, IDocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        public void OnGet()
        {
            Contributors = _documentService.GetDocument(DocumentId).DocumentsSharing.Select(
                document => document.User).ToList();
        }

        public IActionResult OnPostAddContributor(string email, string documentId)
        {
            DocumentId = documentId;
            _documentService.AddContributor(DocumentId, email);
            return RedirectToPage("EditDocument", new {id = DocumentId});
        }

        public IActionResult OnPostRemoveContributor(string email, string documentId)
        {
            DocumentId = documentId;
            _documentService.RemoveContributor(DocumentId, email);

            return RedirectToPage("EditDocument", new {id = DocumentId});
        }
    }
}