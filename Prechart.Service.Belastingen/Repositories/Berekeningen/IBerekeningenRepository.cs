using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Belastingen.Repositories.Berekeningen;

public interface IBerekeningenRepository :
    IHandlerAsync<BerekeningenRepository.UpsertBerekeningen, IFluentResults<bool>>
{
}
