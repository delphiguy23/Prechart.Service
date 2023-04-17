using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Prechart.Service.Email.Service;
using Prechart.Service.Globals.Models.Email;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Email.Controllers;

[Route("/platform/service/api/email/")]
[ApiController]
[Authorize(Roles = "SuperAdmin")]
public class EmailController : ControllerBase
{
    private readonly IEmailRepository _mailRepo;
    private readonly IEmailService _mailService;

    public EmailController(IEmailService mailService, IEmailRepository mailRepo)
    {
        _mailService = mailService;
        _mailRepo = mailRepo;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromForm] EmailToSend request)
    {
        List<EmailAttachmentModel> files = new();
        var streamFile = new MemoryStream();

        if (request.Attachments is not null)
        {
            foreach (var attachment in request.Attachments)
            {
                await attachment.CopyToAsync(streamFile);
                files.Add(new EmailAttachmentModel { Filename = attachment.FileName, FileData = streamFile?.ToArray() ?? null, ContentType = ContentType.Parse(attachment.ContentType) });
            }
        }


        var result = await _mailService.HandleAsync(new EmailService.AddToEmailArchive
        {
            Subject = request.Subject,
            Body = request.Body,
            To = request.To,
            Cc = request.Cc,
            Bcc = request.Bcc,
            Attachments = files,
            EmailEventType = request.EmailEventType,
        }, CancellationToken.None);

        return result.ToActionResult();
    }

    [HttpGet("eventType")]
    public async Task<IActionResult> GetEmailEventType(int eventType)
    {
        var getEmailRecipient = await _mailRepo.HandleAsync(new EmailRepository.GetEmailEventType
        {
            EmailEventType = eventType,
        }, CancellationToken.None);

        return getEmailRecipient.ToActionResult();
    }

    [HttpPost("upserteventtype")]
    public async Task<IActionResult> InsertEmailEventType(EmailEventRecipient request)
    {
        var emailRecipient = new EmailEventRecipientModel
        {
            Recipient = request.Recipient,
            Cc = request.Cc,
            Bcc = request.Bcc,
            EmailEventType = request.EmailEventType,
            Name = request.Name,
            IsActive = request.IsActive,
            IsDeleted = request.IsDeleted,
        };

        var getEmailRecipient = await _mailService.HandleAsync(new EmailService.UpsertEmailEventRecipient
        {
            EmailEvent = emailRecipient
        }, CancellationToken.None);

        return getEmailRecipient.ToActionResult();
    }
}
