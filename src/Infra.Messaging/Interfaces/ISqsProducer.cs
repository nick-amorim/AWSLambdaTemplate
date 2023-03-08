namespace Infra.Messaging.Interfaces;

public interface ISqsProducer
{
    Task SendMessageAsync(string messageBody);
}