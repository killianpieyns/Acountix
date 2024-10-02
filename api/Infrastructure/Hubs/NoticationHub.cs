using Microsoft.AspNetCore.SignalR;

namespace api.Infrastructure.Hubs;
public class NotificationHub : Hub
{
    public async Task SendNotification(string user, string message)
    {
        await Clients.User(user).SendAsync("ReceiveNotification", message);
    }
}