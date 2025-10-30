using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.SignalRHub;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagement_CoreAPI.BLL.Services.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUser(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendToAll(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
