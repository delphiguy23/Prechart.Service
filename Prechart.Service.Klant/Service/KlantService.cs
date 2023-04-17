using MassTransit;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Klant.Repository;

namespace Prechart.Service.Klant.Service;

public partial class KlantService : IKlantService
{
    private readonly ILogger<KlantService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IKlantRepository _repository;

    public KlantService(ILogger<KlantService> logger, IKlantRepository repository, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IFluentResults<List<KlantModel>>> HandleAsync(UpsertKlants request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Start executing {nameof(UpsertKlants)}");

        try
        {
            if (request is null || request.Klants is null || !request.Klants.Any())
            {
                return ResultsTo.NotFound<List<KlantModel>>().WithMessage("Argument is null or empty");
            }

            var result = await _repository.HandleAsync(new KlantRepository.UpsertKlants
            {
                Klants = request.Klants,
            }, cancellationToken);

            if (result.Status == FluentResultsStatus.NotFound)
            {
                return ResultsTo.NotFound<List<KlantModel>>().WithMessage("Client not found");
            }

            foreach (var k in result.Value)
            {
                foreach (var w in k.Werkgevers.Distinct().ToList())
                {
                    await _publishEndpoint.Publish<IUpsertWerkgever>(new UpsertWerkgeverModel
                    {
                        KlantId = k.Id.ToString(),
                        KlantName = k.KlantNaam,
                        TaxNo = w,
                    });
                }
            }

            return ResultsTo.Success<List<KlantModel>>().FromResults(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<KlantModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<KlantModel>>> HandleAsync(GetKlants request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Start executing {nameof(GetKlants)}");

        try
        {
            var result = await _repository.HandleAsync(new KlantRepository.GetKlants(), cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<KlantModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<KlantModel>> HandleAsync(GetKlant request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start executing {nameof(GetKlant)}");

            if (string.IsNullOrWhiteSpace(request.KlantId) && string.IsNullOrWhiteSpace(request.KlantName) && string.IsNullOrWhiteSpace(request.TaxNo))
            {
                return ResultsTo.NotFound<KlantModel>().WithMessage("Argument is null or empty");
            }

            if (!string.IsNullOrWhiteSpace(request.KlantName) && await _repository.HandleAsync(new KlantRepository.GetKlantByName
                {
                    KlantName = request.KlantName,
                }, cancellationToken) is { } resultName && resultName.IsSuccess())
            {
                return ResultsTo.Something(resultName);
            }

            if (!string.IsNullOrWhiteSpace(request.KlantId) && await _repository.HandleAsync(new KlantRepository.GetKlantById
                {
                    KlantId = request.KlantId,
                }, cancellationToken) is { } resultId && resultId.IsSuccess())
            {
                return ResultsTo.Something(resultId);
            }

            if (!string.IsNullOrWhiteSpace(request.TaxNo) && await _repository.HandleAsync(new KlantRepository.GetKlantByTaxNo
                {
                    TaxNo = request.TaxNo,
                }, cancellationToken) is { } resultTaxNo && resultTaxNo.IsSuccess())
            {
                return ResultsTo.Something(resultTaxNo);
            }

            return ResultsTo.NotFound<KlantModel>().WithMessage("Skipped all checks");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<KlantModel>().FromException(e);
        }
    }
}
