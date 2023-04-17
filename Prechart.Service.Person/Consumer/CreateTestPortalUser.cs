using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Person.Services.Person;

namespace Prechart.Service.Person.Consumer;

public class CreateTestPortalUser : IConsumer<IPersonTestUser>
{
    private readonly ILogger<CreateTestPortalUser> _logger;
    private readonly IPersonService _service;

    public CreateTestPortalUser(ILogger<CreateTestPortalUser> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IPersonTestUser> context)
    {
        _logger.LogInformation($"{nameof(IPersonTestUser)} event with payload {context.Message}");

        try
        {
            await _service.HandleAsync(new PersonService.CreateTestUser(), CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
