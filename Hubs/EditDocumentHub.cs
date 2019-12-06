using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterDocs.Areas.Identity;
using BetterDocs.Services;
using BetterDocs.Utils;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Hubs
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditDocumentHub : Hub
    {
        private readonly IDocumentService _documentService;
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EditDocumentHub> _logger;

        public EditDocumentHub(IDocumentService documentService, IDistributedCache distributedCache,
            IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager,
            ILogger<EditDocumentHub> logger)
        {
            _documentService = documentService;
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> StartEditingDocument(string documentId)
        {
            _logger.LogInformation("Request for editing document {} by user {}", documentId,
                GetUserAsync().Result.Email);
            if (!await HasUserAccessToDocument(documentId))
            {
                _logger.LogWarning("Request rejected. User has no permission to access the file");
                return null;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, documentId);
            _logger.LogInformation("User connected to hub establishing connection with id {}", Context.ConnectionId);

            return GetDocumentText(documentId);
        }

        public async Task ChangeText(string text, string documentId)
        {
            _logger.LogInformation("User {} requested change to the document {}:\n {}", GetUserAsync().Result.Email,
                documentId, text);
            if (!await HasUserAccessToDocument(documentId))
            {
                _logger.LogWarning("Request rejected. User has no permission to access the file");
                return;
            }

            _logger.LogInformation("Storing changed text in cache");
            _distributedCache.SetString("documents/" + documentId + "/text", text);

            _logger.LogInformation("Synchronization with other clients...");
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

            _logger.LogInformation("Text has been saved, user {}, documentId {}", GetUserAsync().Result.Email,
                documentId);
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
            var user = await GetUserAsync();
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
                if (textDocument.DocumentsSharing != null)
                {
                    usersWithAccess.AddRange(textDocument.DocumentsSharing.Select(x => x.UserId));
                }

                _distributedCache.Set("documents/" + documentId + "/users", usersWithAccess.SerializeToByteArray(),
                    new DistributedCacheEntryOptions {SlidingExpiration = TimeSpan.FromMinutes(1)});

                return usersWithAccess.Contains(user.Id);
            }
        }

        private async Task<ApplicationUser> GetUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
    }
}