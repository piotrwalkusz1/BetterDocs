using System.Threading.Tasks;
using BetterDocs.Areas.Identity;
using BetterDocs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace BetterDocs.Hubs
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditDocumentHub : Hub
    {
        private readonly DocumentEditionService _documentEditionService;
        private readonly UserManager<ApplicationUser> _userManager;       
        public EditDocumentHub(DocumentEditionService documentEditionService, UserManager<ApplicationUser> userManager)
        {
            _documentEditionService = documentEditionService;
            _userManager = userManager;
        }

        public async Task ChangeText(string text, string documentId)
        {
            var user = _userManager.GetUserAsync(Context.User);
            var textDocument = _documentEditionService.UpdateDocument(user.Result, text, documentId);
            await Clients.All.SendAsync("ChangeText", textDocument.Text);
        }
    }
}