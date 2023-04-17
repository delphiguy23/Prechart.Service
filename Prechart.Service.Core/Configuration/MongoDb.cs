namespace Prechart.Service.Core.Configuration;

public class MongoDb
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }
    public string LogConnectionString { get; set; }
    public string LogDatabase { get; set; }
    public string MailArchiveDatabase { get; set; }
}
