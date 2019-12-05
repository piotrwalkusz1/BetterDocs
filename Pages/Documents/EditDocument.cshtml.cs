using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Pages
{
    public class EditDocumentModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly DocumentService _documentService;

        [FromQuery(Name="Id")]
        public string DocumentId { get; set; }

        public EditDocumentModel(ILogger<PrivacyModel> logger, DocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        public void OnPost(string email)
        {
            _documentService.AddContributor(DocumentId, email);
        }
    }
}