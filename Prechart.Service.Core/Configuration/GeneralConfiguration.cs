namespace Prechart.Service.Core.Configuration;

public class GeneralConfiguration
{
    public string ServiceName { get; set; }
    public string Tenant { get; set; }
    public string Environment { get; set; }
    public string JwtKey { get; set; }
    public string ConnectionString { get; set; }
    public string HangFireConnectionString { get; set; }
    public MongoDb MongoDb { get; set; }
    public string HostName { get; set; }
    public RabbitMqConfiguration RabbitMq { get; set; }
    public MailConfiguration MailSettings { get; set; }
    public BatchProcessIntervals BatchProcessIntervals { get; set; }
    public string ApplicationInsightsInstrumentationKey { get; set; }
}
