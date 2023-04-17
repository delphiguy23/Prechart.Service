using System.Collections.Generic;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Email.Models;

namespace Prechart.Service.Email.Service;

public interface IEmailService :
    IHandlerAsync<EmailService.AddToEmailArchive, IFluentResults<EmailEventModel>>,
    IHandlerAsync<EmailService.SendPendingEmails, IFluentResults<List<EmailEventModel>>>,
    IHandlerAsync<EmailService.UpsertEmailEventRecipient, IFluentResults>
{
}
