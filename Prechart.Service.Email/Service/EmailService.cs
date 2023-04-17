using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Email.Helpers;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using static Prechart.Service.Email.Repositories.EmailRepository;

namespace Prechart.Service.Email.Service;

public partial class EmailService : IEmailService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IOptions<GeneralConfiguration> _generalConfiguration;
    private readonly ILogger<EmailService> _logger;
    private readonly IEmailRepository _repository;

    public EmailService(IOptions<GeneralConfiguration> generalConfiguration, ILogger<EmailService> logger, IEmailRepository repository,
        IBackgroundJobClient backgroundJobClient
    )
    {
        _generalConfiguration = generalConfiguration;
        _logger = logger;
        _repository = repository;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task<IFluentResults<EmailEventModel>> HandleAsync(AddToEmailArchive request, CancellationToken cancellationToken = default)
    {
        var emailEvent = new EmailEventModel();
        emailEvent.EmailFrom = _generalConfiguration.Value.MailSettings.Sender;
        emailEvent.Sender = _generalConfiguration.Value.MailSettings.SenderName;
        emailEvent.Subject = request.Subject;
        emailEvent.Body = request.Body;
        emailEvent.Attachments = request.Attachments;
        emailEvent.EmailEventType = request.EmailEventType;
        emailEvent.Created = request.Created;

        try
        {
            emailEvent.To = request.To;
            emailEvent.Cc = request.Cc;
            emailEvent.Bcc = request.Bcc;

            if (request.EmailEventType != EmailEventType.Normal)
            {
                var getEmailRecipient = await _repository.HandleAsync(new GetEmailEventType
                {
                    EmailEventType = (int) request.EmailEventType,
                }, cancellationToken);

                if (getEmailRecipient.IsNotFoundOrBadRequest() || getEmailRecipient.IsFailure())
                {
                    return ResultsTo.Something(new EmailEventModel()).WithMessage("Email Recipient is null");
                }

                emailEvent.To = getEmailRecipient.Value.Recipient != null ? getEmailRecipient.Value.Recipient.Split(";").ToList() : null;
                emailEvent.Cc = getEmailRecipient.Value.Cc != null ? getEmailRecipient.Value.Cc.Split(";").ToList() : null;
                emailEvent.Bcc = getEmailRecipient.Value.Bcc != null ? getEmailRecipient.Value.Bcc.Split(";").ToList() : null;
            }


            var result = await _repository.HandleAsync(new InsertEmailEvent
            {
                EmailEvent = emailEvent,
            }, cancellationToken);

            _backgroundJobClient.Schedule(() => HandleAsync(new SendPendingEmails(), cancellationToken), TimeSpan.FromSeconds(10));

            return ResultsTo.Something(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ResultsTo.Failure<EmailEventModel>().FromException(ex);
        }
    }

    public async Task<IFluentResults> HandleAsync(UpsertEmailEventRecipient request, CancellationToken cancellationToken = default)
    {
        var insertEmailEventRecipient = await _repository.HandleAsync(new EmailRepository.UpsertEmailEventRecipient
        {
            EmailEvent = request.EmailEvent,
        }, cancellationToken);

        return insertEmailEventRecipient;
    }

    public async Task<IFluentResults<List<EmailEventModel>>> HandleAsync(SendPendingEmails request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing SendPendingEmails");

        var emailevents = await _repository.HandleAsync(new GetPendingEmails {PendingEmails = false}, cancellationToken);
        var getUpdateEmailResults = new List<EmailEventModel>();

        if (emailevents.IsFailure() || emailevents.IsNotFound())
        {
            return ResultsTo.NotFound<List<EmailEventModel>>("No Pending Emails");
        }

        var smtpConfig = new SmtpModel
        {
            Host = _generalConfiguration.Value.MailSettings.Host,
            Username = _generalConfiguration.Value.MailSettings.UserName,
            Sender = _generalConfiguration.Value.MailSettings.Sender,
            Password = _generalConfiguration.Value.MailSettings.Password,
            Port = Convert.ToInt32(_generalConfiguration.Value.MailSettings.Port),
        };

        if (smtpConfig.Host is null || smtpConfig.Username is null)
        {
            return ResultsTo.BadRequest<List<EmailEventModel>>("Smtp Host/Username is null");
        }

        if (emailevents.Value.Any())
        {
            foreach (var emailevent in emailevents.Value)
            {
                var emailsend = await EmailHelper.SendEmail(
                    new EmailEventModel
                    {
                        Id = emailevent.Id,
                        Subject = emailevent.Subject,
                        Body = emailevent.Body,
                        EmailEventType = emailevent.EmailEventType,
                        Attachments = emailevent.Attachments,
                        Bcc = emailevent.Bcc,
                        Cc = emailevent.Cc,
                        Created = emailevent.Created,
                        Sent = emailevent.Sent,
                        To = emailevent.To,
                        Error = emailevent.Error,
                    },
                    smtpConfig);

                var getUpdateEmailEvent = await _repository.HandleAsync(new UpdateEmailEvent
                {
                    Id = emailevent.Id,
                    Error = emailsend.Error,
                    Sent = emailsend.Sent,
                }, cancellationToken);

                getUpdateEmailResults.Add(getUpdateEmailEvent.Value);
            }
        }

        _logger.LogInformation("Done Executing SendPendingEmails");

        return ResultsTo.Success<List<EmailEventModel>>();
    }
}
