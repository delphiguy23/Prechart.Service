using Microsoft.AspNetCore.Http;
using Prechart.Service.Core.Events;
using Prechart.Service.Core.Models;
using Prechart.Service.Globals.Interfaces;
using Prechart.Service.Globals.Models.Email;
using System.Collections.Generic;

namespace Prechart.Service.Documents.Upload.Models
{
    public class EmailEventModel : IEmailEvent
    {
        public string EmailFrom { get; set; }
        public string Sender { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachmentModel> Attachments { get; set; }
        public EmailEventType EmailEventType { get; set; }
    }
}