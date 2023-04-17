using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Prechart.Service.Core.Models;
using Prechart.Service.Globals.Models.Email;

namespace Prechart.Service.Email.Models;

public class EmailToSend
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> To { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public List<IFormFile> Attachments { get; set; }
    public EmailEventType EmailEventType { get; set; }

}