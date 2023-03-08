using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Domain.Service.Notifications.SendMessage;
using Infra.IoC;
using Infra.IoC.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MessageProcessor;

public class Function
{
    private readonly IServiceCollection _serviceCollection;
    private readonly ServiceProvider _serviceProvider;
    private readonly IMediator _mediator;
    private ILogger<Function> _logger;

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        this._serviceCollection = new ServiceCollection()
            .AddServices();
        this._serviceProvider = this._serviceCollection.BuildServiceProvider();
        this._mediator = this._serviceProvider.GetRequiredService<IMediator>();
        this._logger = _serviceProvider.GetRequiredService<ILogger<Function>>();
    }

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        _logger.LogInformation("Starting Lambda");
        try
        {
            foreach (var message in evnt.Records)
            {
                _logger.LogInformation("Starting message {MessageId} processing", message.MessageId);
                await _mediator.Publish(new SendMessageNotification(message.Body));
                _logger.LogInformation("Message {message.MessageId} sent", message.MessageId);
            }

            _logger.LogInformation("Lambda Execution Completed");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred: {Message}", ex.Message);
        }
    }
}