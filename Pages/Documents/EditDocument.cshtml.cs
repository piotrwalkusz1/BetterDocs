using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Pages
{
    public class EditDocumentModel : PageModel
    {
        private readonly ILogger<EditDocumentModel> _logger;

        public EditDocumentModel(ILogger<EditDocumentModel> logger)
        {
            _logger = logger;
        }
    }
}