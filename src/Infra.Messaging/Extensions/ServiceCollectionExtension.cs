using Amazon.SQS;
using Infra.Messaging.Interfaces;
using Infra.Messaging.Sqs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Messaging.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSqsConfiguration(this IServiceCollection services)
    {
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            services.Configure<SqsConfiguration>(configuration.GetSection("Sqs:DestinationQueue"));
        }
        services.AddAWSService<IAmazonSQS>();
        services.AddSingleton<ISqsProducer, SqsProducer>();
        return services;
    }
}