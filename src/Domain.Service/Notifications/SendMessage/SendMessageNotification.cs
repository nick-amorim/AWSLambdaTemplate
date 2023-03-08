using MediatR;

namespace Domain.Service.Notifications.SendMessage;

public class SendMessageNotification : INotification
{
    public SendMessageNotification(string message)
    {
        Message = message;
    }

    public string Message { get; }
}