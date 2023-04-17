namespace Prechart.Service.Email.Models;

public class EmailEventRecipient
{
    public string Recipient { get; set; }
    public string Cc { get; set; }
    public string Bcc { get; set; }
    public string Name { get; set; }
    public int EmailEventType { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}