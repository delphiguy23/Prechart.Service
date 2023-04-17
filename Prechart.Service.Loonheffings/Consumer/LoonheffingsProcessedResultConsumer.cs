using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Loonheffings.Service;

namespace Prechart.Service.Loonheffings.Consumer;

public class LoonheffingsProcessedResultConsumer : IConsumer<ILoonaangifteProcessResult>
{
    private readonly ILogger<LoonheffingsProcessedResultConsumer> _logger;
    private readonly ILoonheffingsService _service;

    public LoonheffingsProcessedResultConsumer(ILogger<LoonheffingsProcessedResultConsumer> logger, ILoonheffingsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<ILoonaangifteProcessResult> context)
    {
        if (context.Message is not null)
        {
            _logger.LogInformation($"{nameof(ILoonaangifteProcessResult)} event with payload {context.Message}");

            try
            {
                await _service.HandleAsync(new LoonheffingsService.LoonheffingsProcessedResult
                {
                    FileName = context.Message.FileName,
                    Processed = context.Message.Processed,
                    ProcessErrors = context.Message.ProcessErrors,
                    EmployeesInserted = context.Message.EmployeesInserted,
                    EmployeesUpdated = context.Message.EmployeesUpdated,
                }, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Consume));
            }
        }
    }
}
