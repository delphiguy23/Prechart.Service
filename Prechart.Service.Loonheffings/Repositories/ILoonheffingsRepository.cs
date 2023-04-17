using System.Collections.Generic;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Loonheffings.Models;

namespace Prechart.Service.Loonheffings.Repositories;

public interface ILoonheffingsRepository :
    IHandlerAsync<LoonheffingsRepository.UpsertTaxFiling2022, IFluentResults>,
    IHandlerAsync<LoonheffingsRepository.GetUnProcessUploadedTaxFiling, IFluentResults<List<UnprocessedUploads>>>,
    IHandlerAsync<LoonheffingsRepository.UpdateProcessedXml, IFluentResults<UpdateResult>>,
    IHandlerAsync<LoonheffingsRepository.GetUnProcessUploadedTaxFilingWithoutEmployees, IFluentResults>
{
}
