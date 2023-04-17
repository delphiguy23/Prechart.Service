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

public class ResetPersonUserCredenitalsConsumer : IConsumer<IResetPersonUserCredentials>
{
    private readonly ILogger<ResetPersonUserCredenitalsConsumer> _logger;
    private readonly IPersonService _service;

    public ResetPersonUserCredenitalsConsumer(ILogger<ResetPersonUserCredenitalsConsumer> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IResetPersonUserCredentials> context)
    {
        _logger.LogInformation($"{nameof(IResetPersonUserCredentials)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new PersonService.ResetPersonUserCredentials
            {
                UserName = context.Message.UserName,
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