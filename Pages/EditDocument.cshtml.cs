using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Pages
{
    public class EditDocumentModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public EditDocumentModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}