using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;

public interface IPremiePercentagesSocialeVerzekeringenService :
    IHandlerAsync<PremiePercentagesSocialeVerzekeringenService.GetPremiePercentage, IFluentResults<PremieBedragModel>>
{
}
