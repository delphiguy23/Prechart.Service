using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Context;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Repositories.Berekeningen;

public partial class BerekeningenRepository : IBerekeningenRepository
{
    private readonly IBelastingenDbContext _context;
    private readonly ILogger<BerekeningenRepository> _logger;
    private readonly IMapper _mapper;

    public BerekeningenRepository(ILogger<BerekeningenRepository> logger, IBelastingenDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpsertBerekeningen request, CancellationToken cancellationToken)
    {
        var result = await _context.Berekeningen.FirstOrDefaultAsync(b => b.Id == request.Berekeningen.Id);

        if (result is null)
        {
            var berekeningen = _mapper.Map<Database.Models.Berekeningen.Berekeningen>(request.Berekeningen);
            await _context.Berekeningen.AddAsync(berekeningen);
        }
        else
        {
            _mapper.Map(request.Berekeningen, result);
        }

        return ResultsTo.Success<bool>(await _context.SaveChangesAsync() > 0);
    }
}
