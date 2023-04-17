using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Email.Models;
using System.Collections.Generic;
using static Prechart.Service.Email.Repositories.EmailRepository;

namespace Prechart.Service.Email.Repositories;

public interface IEmailRepository :
    IHandlerAsync<GetEmailEventType, IFluentResults<EmailEventRecipientModel>>,
    IHandlerAsync<InsertEmailEvent, IFluentResults<EmailEventModel>>,
    IHandlerAsync<UpdateEmailEvent, IFluentResults<EmailEventModel>>,
    IHandlerAsync<UpsertEmailEventRecipient, IFluentResults>,
    IHandlerAsync<GetPendingEmails, IFluentResults<List<EmailEventModel>>>
{
}
