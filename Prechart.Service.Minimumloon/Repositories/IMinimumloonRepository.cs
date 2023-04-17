using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System.Collections.Generic;

namespace Prechart.Service.Minimumloon.Repositories;

public interface IMinimumloonRepository :
    IHandlerAsync<MinimumloonRepository.UpsertMinimumLoon, IFluentResults>,
    IHandlerAsync<MinimumloonRepository.GetMinimumloon, IFluentResults<List<Database.Models.Minimumloon>>>,
    IHandlerAsync<MinimumloonRepository.DeleteMinimumloon, IFluentResults>
{
}
