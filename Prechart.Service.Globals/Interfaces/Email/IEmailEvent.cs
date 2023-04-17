using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Prechart.Service.Core.Models;
using Prechart.Service.Globals.Models.Email;

namespace Prechart.Service.Globals.Interfaces;

public interface IEmailEvent
{
    string EmailFrom { get; } //email of sender
    string Sender { get; } //senders name
    List<string> To { get; }
    List<string> Cc { get; }
    List<string> Bcc { get; }
    string Subject { get; }
    string Body { get; }
    List<EmailAttachmentModel> Attachments { get; }
    EmailEventType EmailEventType { get; }
}
