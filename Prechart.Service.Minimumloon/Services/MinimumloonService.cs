using Microsoft.Extensions.Logging;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Minimumloon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon.Services;

public partial class MinimumloonService : IMinimumloonService
{
    private readonly ILogger<MinimumloonService> _logger;
    private readonly IMinimumloonRepository _repository;

    public MinimumloonService(ILogger<MinimumloonService> logger,
        IMinimumloonRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults> HandleAsync(UpsertMinimumLoon request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.Minimumloon is null || !request.Minimumloon.Any())
            {
                return ResultsTo.BadRequest();
            }

            return await _repository.HandleAsync(new MinimumloonRepository.UpsertMinimumLoon { Minimumloon = request.Minimumloon }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<int>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<Database.Models.Minimumloon>>> HandleAsync(GetMinimumloon request, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _repository.HandleAsync(new MinimumloonRepository.GetMinimumloon { Datum = request.Datum }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<List<Database.Models.Minimumloon>>().FromException(e);
        }
    }

    public async Task<IFluentResults> HandleAsync(DeleteMinimumloon request, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _repository.HandleAsync(new MinimumloonRepository.DeleteMinimumloon { Datum = request.Datum, Leeftijd = request.Leeftijd }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<int>().FromException(e);
        }
    }
}
