namespace Prechart.Service.Core.Configuration;

public class MailConfiguration
{
    public string Sender { get; set; }
    public string SenderName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public bool Ssl { get; set; }
}