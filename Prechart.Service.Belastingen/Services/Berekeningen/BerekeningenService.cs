using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Interfaces.PremieBedrag;
using Prechart.Service.Belastingen.Helpers;
using Prechart.Service.Belastingen.Models.Berekeningen;
using Prechart.Service.Belastingen.Repositories.Berekeningen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Interfaces.PremieBedrag;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Belastingen;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Services.Berekeningen;

public partial class BerekeningenService : IBerekeningenService
{
    private readonly IRequestClient<IGetInhouding> _getInhoudingEvent;
    private readonly IRequestClient<IGetPremieBedrag> _getPremieBedragEvent;
    private readonly IRequestClient<IMongoGetWerkgever> _getWerkgeverEvent;
    private readonly ILogger<BerekeningenService> _logger;
    private readonly IBerekeningenRepository _repository;

    public BerekeningenService(ILogger<BerekeningenService> logger,
        IBerekeningenRepository repository,
        IRequestClient<IMongoGetWerkgever> getWerkgeverEvent,
        IRequestClient<IGetInhouding> getInhoudingEvent,
        IRequestClient<IGetPremieBedrag> getPremieBedragEvent)
    {
        _logger = logger;
        _repository = repository;
        _getWerkgeverEvent = getWerkgeverEvent;
        _getInhoudingEvent = getInhoudingEvent;
        _getPremieBedragEvent = getPremieBedragEvent;
    }

    public async Task<IFluentResults<BerekeningenModel>> HandleAsync(CalculateBerekenen request, CancellationToken cancellationToken)
    {
        var getInhouding = BerekeningenHelpers.InitializeGetInhoudingEvent(request);
        var getPremieBedrag = BerekeningenHelpers.InitializegetGetPremieBedragEvent(request, getInhouding.ProcesDatum);
        var getWhkWerkgever = BerekeningenHelpers.InitializeGetWhkWerkgeverEvent(request);

        var inhoudingEvent = _getInhoudingEvent.GetResponse<IBerekenenInhouding, NotFound>(getInhouding);
        var premieBedragEvent = _getPremieBedragEvent.GetResponse<IPremieBedrag, NotFound>(getPremieBedrag);
        var whkWerkgeverEvent = _getWerkgeverEvent.GetResponse<IMongoWerkgevers, NotFound>(getWhkWerkgever);

        _logger.LogInformation("Fetching data for Berekeningen Calculation");
        await Task.WhenAll(inhoudingEvent, premieBedragEvent, whkWerkgeverEvent);
        _logger.LogInformation("Done fetching data for Berekeningen Calculation");

        var inhoudingResponse = inhoudingEvent.Result;
        var premieBedragResponse = premieBedragEvent.Result;
        var whkWerkgeverResponse = whkWerkgeverEvent.Result;

        if (inhoudingResponse.Is(out Response<IBerekenenInhouding> inhouding) &&
            premieBedragResponse.Is(out Response<IPremieBedrag> premieBedrag) &&
            whkWerkgeverResponse.Is(out Response<IMongoWerkgevers> whkWerkgever))
        {
            var berekeningen = new BerekeningenModel();

            berekeningen = BerekeningenHelpers.InitBerekeningen(request, berekeningen);
            berekeningen = BerekeningenHelpers.ApplyInhoudingDataToBerekeningen(berekeningen, inhouding);
            berekeningen = BerekeningenHelpers.ApplyPremieBedragDataToBerekiningen(request, berekeningen, premieBedrag);
            berekeningen = BerekeningenHelpers.ApplyWhkWerkgeverDataToBerekiningen(berekeningen, whkWerkgever, request.Parameters.InhoudingParameters.ProcesDatum);
            berekeningen.EmployeeId = request.Parameters.EmployeeId;

            await _repository.HandleAsync(new BerekeningenRepository.UpsertBerekeningen { Berekeningen = berekeningen }, CancellationToken.None);
            return ResultsTo.Success<BerekeningenModel>(berekeningen);
        }

        return ResultsTo.NotFound<BerekeningenModel>();
    }
}
