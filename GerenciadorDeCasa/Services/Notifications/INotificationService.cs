namespace GerenciadorDeCasa.Services.Notifications;

public interface INotificationService
{
    Task SendNotificationAsync(string contact, string title, string message);
}
