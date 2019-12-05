using System.Threading.Tasks;
using BetterDocs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BetterDocs.Hubs
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditDocumentHub : Hub
    {
        private readonly DocumentService _documentService;

        public EditDocumentHub(DocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task ChangeText(string text, string documentId)
        {
            var textDocument = _documentService.UpdateDocument(text, documentId);
            // TODO: Nie wysyłaj do wszystkich, tylko do osób, które mają updarwnienia do dokumentu
            await Clients.All.SendAsync("ChangeText", textDocument.Text, documentId);
        }
    }
}