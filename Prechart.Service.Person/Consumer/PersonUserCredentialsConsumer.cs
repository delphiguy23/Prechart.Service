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

public class PersonUserCredentialsConsumer : IConsumer<IPersonUserCredentials>
{
    private readonly ILogger<PersonUserCredentialsConsumer> _logger;
    private readonly IPersonService _service;

    public PersonUserCredentialsConsumer(ILogger<PersonUserCredentialsConsumer> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IPersonUserCredentials> context)
    {
        _logger.LogInformation($"{nameof(IPersonUserCredentials)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new PersonService.PersonCredentialCheck
            {
                Username = context.Message.Username,
                Password = context.Message.Password,
            }, CancellationToken.None);

            if (result is null || result.IsNotFoundOrBadRequest())
            {
                await context.RespondAsync(new NotFound());
            }

            await context.RespondAsync<IPersonCredentialCheck>(new {Id = result?.Value});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}