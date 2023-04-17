using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Person.Services;

namespace Prechart.Service.Person.Consumer;

public class GetPersonsByBsnConsumer : IConsumer<IPersonsByBsn>
{
    private readonly ILogger<IPersonsByBsn> _logger;
    private readonly IPersonService _service;

    public GetPersonsByBsnConsumer(ILogger<IPersonsByBsn> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IPersonsByBsn> context)
    {
        try
        {
            var result = await _service.Handle(new PersonService.UpsertPersons
            {
                Persons = context.Message,
            }) as IsSome<IPersonUpsertResults>;

            if (result is null)
            {
                await context.RespondAsync(new NotFound());
            }
            
            await context.RespondAsync(result?.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}