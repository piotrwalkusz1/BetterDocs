using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterDocs.Areas.Identity;
using BetterDocs.Services;
using BetterDocs.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace BetterDocs.Hubs
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditDocumentHub : Hub
    {
        private readonly DocumentService _documentService;
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditDocumentHub(DocumentService documentService, IDistributedCache distributedCache,
            IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _documentService = documentService;
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<string> StartEditingDocument(string documentId)
        {
            if (!await HasUserAccessToDocument(documentId))
            {
                return null;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, documentId);

            return GetDocumentText(documentId);
        }

        public async Task ChangeText(string text, string documentId)
        {
            if (!await HasUserAccessToDocument(documentId))
            {
                return;
            }

            _distributedCache.SetString("documents/" + documentId + "/text", text);

            await Clients.GroupExcept(documentId, Context.ConnectionId).SendAsync("ChangeText", text);
        }

        public async Task SaveText(string documentId)
        {
            if (!await HasUserAccessToDocument(documentId))
            {
                return;
            }

            var text = GetDocumentText(documentId);
            _documentService.UpdateDocument(text, documentId);
        }

        private string GetDocumentText(string documentId)
        {
            var text = _distributedCache.GetString("documents/" + documentId + "/text");

            if (text != null)
            {
                return text;
            }

            text = _documentService.GetDocumentWithoutCheckingAccess(documentId).Text;

            _distributedCache.SetString("documents/" + documentId + "/text", text);

            return text;
        }

        private async Task<bool> HasUserAccessToDocument(string documentId)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var usersWithAccessAsBytes = _distributedCache.Get("documents/" + documentId + "/users");

            if (usersWithAccessAsBytes != null)
            {
                var usersWithAccess = usersWithAccessAsBytes.Deserialize<List<string>>();
                return usersWithAccess.Contains(user.Id);
            }
            else
            {
                var textDocument = _documentService.GetDocumentWithoutCheckingAccess(documentId);

                var usersWithAccess = new List<string> {textDocument.Owner.Id};
                if (textDocument.SharedWith != null)
                {
                    usersWithAccess.AddRange(textDocument.SharedWith.Select(userSharedWith => userSharedWith.Id));
                }

                _distributedCache.Set("documents/" + documentId + "/users", usersWithAccess.SerializeToByteArray(),
                    new DistributedCacheEntryOptions {SlidingExpiration = TimeSpan.FromMinutes(1)});

                return usersWithAccess.Contains(user.Id);
            }
        }
    }
}