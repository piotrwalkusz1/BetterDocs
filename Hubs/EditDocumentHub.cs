using System.Threading.Tasks;
using BetterDocs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BetterDocs.Hubs
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditDocumentHub : Hub
    {
        private readonly DocumentEditionService _documentEditionService;
        public EditDocumentHub(DocumentEditionService documentEditionService)
        {
            _documentEditionService = documentEditionService;
        }

        public async Task ChangeText(string text, string documentId)
        {
            var textDocument = _documentEditionService.UpdateDocument(text, documentId);
            await Clients.All.SendAsync("ChangeText", textDocument.Text);
        }
    }
}