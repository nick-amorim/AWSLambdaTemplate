using Amazon.SQS;
using Amazon.SQS.Model;
using Infra.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infra.Messaging.Sqs;

public class SqsProducer : ISqsProducer
{
    private readonly IAmazonSQS _sqsClient;
    private readonly ILogger<SqsProducer> _logger;
    private readonly SqsConfiguration _sqsConfig;

    public  SqsProducer(IAmazonSQS sqsClient, IOptionsSnapshot<SqsConfiguration> configuration, 
        ILogger<SqsProducer> logger)
    {
        _sqsClient = sqsClient;
        _sqsConfig = configuration.Value;
        _logger = logger;
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            var queue =  await _sqsClient.GetQueueUrlAsync(_sqsConfig.AwsQueueName);
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queue.QueueUrl,
                MessageBody = message
            };

            await _sqsClient.SendMessageAsync(sendMessageRequest);

            _logger.LogInformation("Message sent to SQS queue: {QueueUrl}. Message: {Message}", queue.QueueUrl, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message: {Message} to queue {Queue}", message, _sqsConfig.AwsQueueName);
            throw;
        }
    }
}