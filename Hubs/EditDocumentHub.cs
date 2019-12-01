using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BetterDocs.Hubs
{
    public class EditDocumentHub : Hub
    {
        public async Task ChangeText(string text)
        {
            await Clients.Others.SendAsync("ChangeText", text);
        }
    }
}