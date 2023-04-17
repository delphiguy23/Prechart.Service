using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Klant;

namespace Prechart.Service.Klant.Repository;

public interface IKlantRepository :
    IHandlerAsync<KlantRepository.UpsertKlants, IFluentResults<List<KlantModel>>>,
    IHandlerAsync<KlantRepository.GetKlants, IFluentResults<List<KlantModel>>>,
    IHandlerAsync<KlantRepository.GetKlantById, IFluentResults<KlantModel>>,
    IHandlerAsync<KlantRepository.GetKlantByName, IFluentResults<KlantModel>>,
    IHandlerAsync<KlantRepository.GetKlantByTaxNo, IFluentResults<KlantModel>>
{
}