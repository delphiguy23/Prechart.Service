using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Klant;

namespace Prechart.Service.Klant.Service;

public interface IKlantService :
    IHandlerAsync<KlantService.UpsertKlants, IFluentResults<List<KlantModel>>>,
    IHandlerAsync<KlantService.GetKlants, IFluentResults<List<KlantModel>>>,
    IHandlerAsync<KlantService.GetKlant, IFluentResults<KlantModel>>
{
}