namespace FUNewsManagement_CoreAPI.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendToUser(string userId, string message);
        Task SendToAll(string message);
    }
}
