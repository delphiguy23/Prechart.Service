using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;

public interface IPremiePercentagesSocialeVerzekeringenRepository :
    IHandlerAsync<PremiePercentagesSocialeVerzekeringenRepository.GetPremiePercentage, IFluentResults<PremieBedragModel>>
{
}
