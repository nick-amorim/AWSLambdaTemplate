using Domain.Service.Extensions;
using Infra.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infra.IoC.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var configuration = GetConfiguration();
        services.AddSingleton(configuration);
        
        var loggerOptions = new LambdaLoggerOptions(configuration);
        services.AddLogging(configure => configure.AddLambdaLogger(loggerOptions));

        services.AddDomainServices();
        services.AddSqsConfiguration();
        return services;
    }


    private static LambdaLoggerOptions GetLoggerOptions()
    {

       return  new LambdaLoggerOptions
       {
           IncludeCategory = true,
           IncludeLogLevel = true,
           IncludeNewline = true,
           IncludeEventId = true,
           IncludeException = true
       };
    }
    
    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}