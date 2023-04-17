using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Email.Models;
using Prechart.Service.Globals.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Email.Repositories;

public partial class EmailRepository : IEmailRepository
{
    private readonly IMongoCollection<EmailEventModel> _collection;
    private readonly IMongoCollection<EmailEventRecipientModel> _collectionEmailEventRecipient;
    private readonly ILogger<EmailRepository> _logger;
    private readonly IMongoDbHelper _mongoDbHelper;

    public EmailRepository(ILogger<EmailRepository> logger, IMongoCollection<EmailEventModel> collection,
        IMongoCollection<EmailEventRecipientModel> collectionEmailEventRecipient, IMongoDbHelper mongoDbHelper)
    {
        _logger = logger;
        _collection = collection;
        _collectionEmailEventRecipient = collectionEmailEventRecipient;
        _mongoDbHelper = mongoDbHelper;

        _mongoDbHelper.TryClassMapRegistration<EmailEventModel>(typeof(EmailEventModel));
        _mongoDbHelper.TryClassMapRegistration<EmailEventRecipientModel>(typeof(EmailEventRecipientModel));
    }

    public async Task<IFluentResults<EmailEventRecipientModel>> HandleAsync(GetEmailEventType request, CancellationToken cancellationToken)
    {
        try
        {

            if (request is null)
            {
                return ResultsTo.NotFound<EmailEventRecipientModel>();
            }

            var result = await _collectionEmailEventRecipient.FindAsync(k => k.EmailEventType == request.EmailEventType && k.IsActive && !k.IsDeleted).Result.FirstOrDefaultAsync();


            return result is not null ? ResultsTo.Something(result) : ResultsTo.NotFound<EmailEventRecipientModel>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<EmailEventRecipientModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<EmailEventModel>> HandleAsync(InsertEmailEvent request, CancellationToken cancellationToken)
    {
        var emailEvent = request.EmailEvent;

        try
        {
            if (emailEvent is null)
            {
                return ResultsTo.BadRequest<EmailEventModel>("No Records to Insert");
            }

            await _collection.InsertOneAsync(emailEvent);

            return ResultsTo.Success(emailEvent);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<EmailEventModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<EmailEventModel>> HandleAsync(UpdateEmailEvent request, CancellationToken cancellationToken = default)
    {
        try
        {
            var option = new FindOneAndUpdateOptions<EmailEventModel> { ReturnDocument = ReturnDocument.After };

            var result = await _collection.FindOneAndUpdateAsync(
                Builders<EmailEventModel>.Filter.Eq(ev => ev.Id, request.Id),
                Builders<EmailEventModel>.Update
                    .Set(ev => ev.ProcessedOn, DateTime.Now)
                    .Set(ev => ev.Sent, request.Sent)
                    .Set(ev => ev.Error, request.Error), option);

            return result is not null ? ResultsTo.Something(result) : ResultsTo.NotFound<EmailEventModel>("No Records to Update");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<EmailEventModel>().FromException(e);
        }
    }

    public async Task<IFluentResults> HandleAsync(UpsertEmailEventRecipient request, CancellationToken cancellationToken = default)
    {
        try
        {
            var emailEventRecipient = request.EmailEvent;

            if (emailEventRecipient is null)
            {
                return ResultsTo.BadRequest().WithMessage("Email Address is null");
            }

            var emailrecipients = String.IsNullOrWhiteSpace(emailEventRecipient.Recipient) ? new List<string>() : emailEventRecipient.Recipient.Split(";").ToList();

            foreach (var recipient in emailrecipients)
            {
                if (!EmailHelper.IsValidEmail(recipient) || recipient.Length == 0)
                {
                    return ResultsTo.BadRequest().WithMessage("Invalid Email Address");
                }
            }

            var getEmailRecipient = await _collectionEmailEventRecipient.FindAsync(er => er.EmailEventType == emailEventRecipient.EmailEventType && er.IsActive && !er.IsDeleted).Result.FirstOrDefaultAsync();

            if (getEmailRecipient is null)
            {
                await _collectionEmailEventRecipient.InsertOneAsync(emailEventRecipient);

                return ResultsTo.Success().WithMessage("Records Added");
            }

            await _collectionEmailEventRecipient.FindOneAndUpdateAsync(
               Builders<EmailEventRecipientModel>.Filter.Eq(er => er.Id, getEmailRecipient.Id),
               Builders<EmailEventRecipientModel>.Update
                   .Set(er => er.Name, emailEventRecipient.Name)
                   .Set(er => er.Recipient, emailEventRecipient.Recipient)
                   .Set(er => er.Cc, emailEventRecipient.Cc)
                   .Set(er => er.Bcc, emailEventRecipient.Bcc)
                   .Set(er => er.IsActive, emailEventRecipient.IsActive));

            return ResultsTo.Success().WithMessage("Records Updated");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure().FromException(e);
        }
    }

    public async Task<IFluentResults<List<EmailEventModel>>> HandleAsync(GetPendingEmails request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _collection.FindAsync(f => f.Sent == request.PendingEmails).Result.ToListAsync();

            return result.Any() ? ResultsTo.Success(result) : ResultsTo.NotFound<List<EmailEventModel>>();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<List<EmailEventModel>>().FromException(e);
        }
    }
}
