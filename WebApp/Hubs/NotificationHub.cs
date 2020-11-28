using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApp.Hubs
{
    public class NotificationHub : Hub
    {

        public async Task Notify(string message, string userId)
        {
            await Clients.User(userId).SendAsync("Notify", message);
        }

    }
}
