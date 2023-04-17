using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Globals.Interfaces.Belastingen;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Consumers;

public class InsertToTaxTableConsumer : IConsumer<IInsertToTaxTableEvent>
{
    private readonly ILogger<InsertToTaxTableConsumer> _logger;
    private readonly IBelastingTabellenWitGroenService _service;

    public InsertToTaxTableConsumer(ILogger<InsertToTaxTableConsumer> logger, IBelastingTabellenWitGroenService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IInsertToTaxTableEvent> context)
    {
        _logger.LogInformation($"{nameof(IInsertToTaxTableEvent)} event with payload {context.Message}");

        try
        {
            var taxresult = await _service.HandleAsync(new BelastingTabellenWitGroenService.UpsertToTable
            {
                TaxType = context.Message.TaxType,
                TaxTable = context.Message.TaxTable,
            }, CancellationToken.None);

            var result = new InsertToTaxTable();

            if (taxresult is null)
            {
                _logger.LogInformation("taxresult is null");

                await context.RespondAsync<IInsertToTaxTable>(new InsertToTaxTable());
            }

            result.IsProcessed = taxresult.Value;

            await context.RespondAsync<IInsertToTaxTable>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            await context.RespondAsync<IInsertToTaxTable>(new InsertToTaxTable());
        }
    }
}
