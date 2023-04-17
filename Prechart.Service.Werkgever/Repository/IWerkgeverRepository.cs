using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Database.Models;

namespace Prechart.Service.Werkgever.Repository;

public interface IWerkgeverRepository :
    IHandlerAsync<WerkgeverRepository.UpsertWerkgevers, IFluentResults<List<MongoWerkgeverModel>>>,
    IHandlerAsync<WerkgeverRepository.GetMongoWerkgever, IFluentResults<List<MongoWerkgeverModel>>>,
    IHandlerAsync<WerkgeverRepository.UpsertFromMongoWerkgever, IFluentResults<int>>,
    IHandlerAsync<WerkgeverRepository.UpsertFromMongoWerkgeverWhk, IFluentResults>,
    IHandlerAsync<WerkgeverRepository.GetSqlWerkgevers, IFluentResults<List<Database.Models.Werkgever>>>,
    IHandlerAsync<WerkgeverRepository.GetSqlWerkgeversWhkPremies, IFluentResults<List<WerkgeverWhkPremies>>>,
    IHandlerAsync<WerkgeverRepository.UpsertMongoWhk, IFluentResults<bool>>,
    IHandlerAsync<WerkgeverRepository.GetCollectieve, IFluentResults<List<CollectieveAangifteModel>>>,
    IHandlerAsync<WerkgeverRepository.UpdateKlantWerkgever, IFluentResults<bool>>
{
}
