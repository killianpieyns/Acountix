using System.Threading.Tasks;
using api.Infrastructure.Services;
using api.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace api.Infrastructure.Services;

public class NotificationService
{
    private readonly EmailService _emailService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(EmailService emailService, IHubContext<NotificationHub> hubContext)
    {
        _emailService = emailService;
        _hubContext = hubContext;
    }

    public async Task SendEmailNotificationAsync(string to, string subject, string body)
    {
        await _emailService.SendEmailAsync(to, subject, body);
    }

    public async Task SendRealTimeNotificationAsync(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}
