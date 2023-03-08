namespace Infra.Messaging;

public class SqsConfiguration
{
    public string AwsRegion { get; set; }
    public string AwsAccessKey { get; set; }
    public string AwsSecretKey { get; set; }
    public string AwsQueueName { get; set; }
}