using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Werkgever;

namespace Prechart.Service.Werkgever.Service;

public interface IWerkgeverService :
    IHandlerAsync<WerkgeverService.GetMongoWerkgever, IFluentResults<List<MongoWerkgeverModel>>>,
    IHandlerAsync<WerkgeverService.UpsertMongoWerkgevers, IFluentResults<List<MongoWerkgeverModel>>>,
    IHandlerAsync<WerkgeverService.UpsertMongoWhk, IFluentResults<bool>>,
    IHandlerAsync<WerkgeverService.SyncFromSql, IFluentResults<bool>>,
    IHandlerAsync<WerkgeverService.GetCollectieve, IFluentResults<List<CollectieveAangifteModel>>>,
    IHandlerAsync<WerkgeverService.UpdateKlantWerkgever, IFluentResults<bool>>,
    IHandlerAsync<WerkgeverService.Print, IFluentResults<string>>
{
}
