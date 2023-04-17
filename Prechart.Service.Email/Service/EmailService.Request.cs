using Prechart.Service.Core.Models;
using Prechart.Service.Email.Models;
using Prechart.Service.Globals.Models.Email;
using System;
using System.Collections.Generic;

namespace Prechart.Service.Email.Service;

public partial class EmailService : IEmailService
{
    public record AddToEmailArchive
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public List<EmailAttachmentModel> Attachments { get; set; }
        public EmailEventType EmailEventType { get; set; }
        public DateTime? Sent { get; set; }
        public DateTime Created { get; set; }
        public string Error { get; set; }
    }

    public record UpsertEmailEventRecipient
    {
        public EmailEventRecipientModel EmailEvent { get; set; }
    }

    public record SendPendingEmails
    {
    }
}