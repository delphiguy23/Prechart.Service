using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Person.Services.Person;

namespace Prechart.Service.Person.Consumer;

public class UpsertPersonUserCredenitalsConsumer : IConsumer<IUpsertPersonUserCredentials>
{
    private readonly ILogger<UpsertPersonUserCredenitalsConsumer> _logger;
    private readonly IPersonService _service;

    public UpsertPersonUserCredenitalsConsumer(ILogger<UpsertPersonUserCredenitalsConsumer> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IUpsertPersonUserCredentials> context)
    {
        _logger.LogInformation($"{nameof(IUpsertPersonUserCredentials)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new PersonService.UpsertPersonUserCredentials
            {
                PersonId = context.Message.PersonId,
                UserName = context.Message.UserName,
                EmailAddress = context.Message.EmailAddress,
            }, CancellationToken.None);

            if (result is null || result.IsNotFoundOrBadRequest())
            {
                await context.RespondAsync(new NotFound());
            }

            await context.RespondAsync<Status>(new {Result = result?.Value});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}