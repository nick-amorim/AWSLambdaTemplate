using Infra.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Service.Notifications.SendMessage;

public class SendMessageNotificationHandler : INotificationHandler<SendMessageNotification>
{
    private readonly ILogger<SendMessageNotificationHandler> _logger;
    private readonly ISqsProducer _sqsProduce;

    public SendMessageNotificationHandler(ILogger<SendMessageNotificationHandler> logger, ISqsProducer sqsProduce)
    {
        _logger = logger;
        _sqsProduce = sqsProduce;
    }

    public async Task Handle(SendMessageNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message {Message} is being sent", notification.Message);
        await _sqsProduce.SendMessageAsync(notification.Message);
    }
}