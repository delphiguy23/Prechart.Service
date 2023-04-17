using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Prechart.Service.Email.Service;
using Prechart.Service.Globals.Interfaces;

namespace Prechart.Service.Email.Consumers;

public class EmailEventConsumer : IConsumer<IEmailEvent>
{
    private readonly IEmailService _emailService;

    public EmailEventConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<IEmailEvent> context)
    {
        var insertEmail = new EmailService.AddToEmailArchive
        {
            Subject = context.Message.Subject,
            Body = context.Message.Body,
            To = context.Message.To,
            Cc = context.Message.Cc,
            Bcc = context.Message.Bcc,
            Attachments = context.Message.Attachments,
            EmailEventType = context.Message.EmailEventType,
            Created = DateTime.Now,
        };

        await _emailService.HandleAsync(insertEmail, CancellationToken.None);
    }
}
