using Prechart.Service.Belastingen.Models.Berekeningen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Belastingen.Services.Berekeningen;

public interface IBerekeningenService :
    IHandlerAsync<BerekeningenService.CalculateBerekenen, IFluentResults<BerekeningenModel>>
{
}
