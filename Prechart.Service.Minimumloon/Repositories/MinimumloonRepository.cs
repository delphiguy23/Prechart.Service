using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.Service;
using Prechart.Service.Minimumloon.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon.Repositories;

public partial class MinimumloonRepository : IMinimumloonRepository
{
    private readonly IMinimumloonDBContext _context;
    private readonly ILogger<MinimumloonRepository> _logger;
    private readonly IMapper _mapper;
    private readonly Action<int> _timeout;

    public MinimumloonRepository(ILogger<MinimumloonRepository> logger,
        IMinimumloonDBContext context, IMapper mapper)
        : this(logger, context, mapper, to => { context.Database.SetCommandTimeout(to); })
    {
    }

    public MinimumloonRepository(ILogger<MinimumloonRepository> logger,
        IMinimumloonDBContext context, IMapper mapper, Action<int> timeout)
    {
        _logger = logger;
        _context = context;
        _timeout = timeout;
        _mapper = mapper;
    }


    public async Task<IFluentResults> HandleAsync(UpsertMinimumLoon request, CancellationToken cancellationToken)
    {
        try
        {
            _timeout(1000);

            if (request.Minimumloon is null)
            {
                return ResultsTo.BadRequest<None>().WithMessage("Request is empty");
            }

            foreach (var loon in request.Minimumloon)
            {
                var minimimumloon = await _context.Minimumloon.Where(w => w.Jaar == loon.Jaar)
                    .Where(w => w.MinimumloonLeeftijd == loon.MinimumloonLeeftijd)
                    .Where(w => w.MinimumloonRecordActiefVanaf == loon.MinimumloonRecordActiefVanaf)
                    .Where(w => w.MinimumloonRecordActiefTot == loon.MinimumloonRecordActiefTot)
                    .Where(w => w.MinimumloonRecordActief).FirstOrDefaultAsync();

                if (minimimumloon is null)
                {
                    await _context.Minimumloon.AddRangeAsync(_mapper.Map<Database.Models.Minimumloon>(loon));
                }
                else
                {
                    minimimumloon.MinimumloonPerMaand = loon.MinimumloonPerMaand;
                    minimimumloon.MinimumloonPerWeek = loon.MinimumloonPerWeek;
                    minimimumloon.MinimumloonPerDag = loon.MinimumloonPerDag;
                    minimimumloon.MinimumloonPerUur36 = loon.MinimumloonPerUur36;
                    minimimumloon.MinimumloonPerUur38 = loon.MinimumloonPerUur38;
                    minimimumloon.MinimumloonPerUur40 = loon.MinimumloonPerUur40;
                    minimimumloon.MinimumloonRecordActiefVanaf = loon.MinimumloonRecordActiefVanaf;
                    minimimumloon.MinimumloonRecordActiefTot = loon.MinimumloonRecordActiefTot;
                    minimimumloon.MinimumloonRecordActief = loon.MinimumloonRecordActief;
                }
            }

            return await _context.SaveChangesAsync() > 0 ? ResultsTo.Success() : ResultsTo.NotFound<int>("No records updated.");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<int>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<Database.Models.Minimumloon>>> HandleAsync(GetMinimumloon request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            return ResultsTo.BadRequest<List<Database.Models.Minimumloon>>();
        }

        var result = await _context.Minimumloon.Where(m => request.Datum >= m.MinimumloonRecordActiefVanaf)
            .Where(m => request.Datum <= m.MinimumloonRecordActiefTot)
            .Where(m => m.MinimumloonRecordActief)
            .OrderByDescending(m => m.MinimumloonLeeftijd)
            .ToListAsync();

        return result is null ? ResultsTo.NotFound<List<Database.Models.Minimumloon>>() : ResultsTo.Success(result);
    }

    public async Task<IFluentResults> HandleAsync(DeleteMinimumloon request, CancellationToken cancellationToken = default)
    {
        if (request.Leeftijd is null || request.Leeftijd == 0)
        {
            var minimimumloons = await _context.Minimumloon
                .Where(m => m.MinimumloonRecordActiefVanaf <= request.Datum)
                .Where(m => m.MinimumloonRecordActiefTot >= request.Datum)
                .Where(w => w.MinimumloonRecordActief).ToListAsync();

            minimimumloons.ForEach(m => m.MinimumloonRecordActief = false);
        }
        else
        {
            var minimimumloons = await _context.Minimumloon
                .Where(m => request.Datum >= m.MinimumloonRecordActiefVanaf)
                .Where(m => request.Datum <= m.MinimumloonRecordActiefTot)
                .Where(w => w.MinimumloonRecordActief)
                .FirstOrDefaultAsync(m => m.MinimumloonLeeftijd == request.Leeftijd);

            minimimumloons.MinimumloonRecordActief = false;
        }

        return await _context.SaveChangesAsync() > 0 ? ResultsTo.Success() : ResultsTo.NotFound("No records updated.");
    }
}
