using Microsoft.AspNetCore.Mvc;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Interfaces.Documents;
using static Prechart.Service.Documents.Upload.Csv.Services.DocumentsService;

namespace Prechart.Service.Documents.Upload.Csv.Services;

public interface IDocumentsService :
    IHandlerAsync<ProcessUploadedCsvFiles, IFluentResults<IInsertToTaxResultMessage>>,
    IHandlerAsync<ProcessUploadedXmlFiles, IFluentResults<ILoonaangiftesUploadResults>>,
    IHandlerAsync<UpdatePersonPhoto, IFluentResults<bool>>,
    IHandlerAsync<DownloadPersonPhoto, IFluentResults<FileContentResult>>
{
}
