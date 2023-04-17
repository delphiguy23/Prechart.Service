using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Person.Services.Person;

namespace Prechart.Service.Person.Consumer;

public class XmlToPersonConsumer : IConsumer<IXmlToPersons>
{
    private readonly ILogger<XmlToPersonConsumer> _logger;
    private readonly IPersonService _service;

    public XmlToPersonConsumer(ILogger<XmlToPersonConsumer> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IXmlToPersons> context)
    {
        _logger.LogInformation($"{nameof(IXmlToPersons)} event with payload {context.Message}");

        try
        {
            await _service.HandleAsync(new PersonService.UpsertPersons
            {
                Persons = context.Message,
            }, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}