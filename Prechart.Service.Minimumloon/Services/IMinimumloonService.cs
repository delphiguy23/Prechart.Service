using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System.Collections.Generic;

namespace Prechart.Service.Minimumloon.Services;

public interface IMinimumloonService :
    IHandlerAsync<MinimumloonService.UpsertMinimumLoon, IFluentResults>,
    IHandlerAsync<MinimumloonService.GetMinimumloon, IFluentResults<List<Database.Models.Minimumloon>>>,
    IHandlerAsync<MinimumloonService.DeleteMinimumloon, IFluentResults>
{
}
