using System;
using System.Threading.Tasks;
using BetterDocs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace BetterDocs.Hubs
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditDocumentHub : Hub
    {
        private readonly DocumentService _documentService;
        private readonly IDistributedCache _distributedCache;

        public EditDocumentHub(DocumentService documentService, IDistributedCache distributedCache)
        {
            _documentService = documentService;
            _distributedCache = distributedCache;
        }

        public String GetText(string documentId)
        {
            return _documentService.GetDocument(documentId).Text;
        }

        public async Task ChangeText(string text, string documentId)
        {
            var textDocument = _documentService.UpdateDocument(text, documentId);
            // TODO: Nie wysyłaj do wszystkich, tylko do osób, które mają updarwnienia do dokumentu
            await Clients.All.SendAsync("ChangeText", textDocument.Text, documentId);
        }
    }
}