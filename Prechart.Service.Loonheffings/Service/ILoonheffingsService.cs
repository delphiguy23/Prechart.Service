using System.Collections.Generic;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Loonheffings.Models;

namespace Prechart.Service.Loonheffings.Service;

public interface ILoonheffingsService :
    IHandlerAsync<LoonheffingsService.ValidateXml, IFluentResults<UploadXmlResults>>,
    IHandlerAsync<LoonheffingsService.ProcessUploadedXmls, IFluentResults<List<ProcessXmlResult>>>,
    IHandlerAsync<LoonheffingsService.LoonheffingsProcessedResult, IFluentResults<UpdateResult>>
{
}