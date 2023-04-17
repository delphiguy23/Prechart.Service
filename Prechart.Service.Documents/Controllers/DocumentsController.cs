using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Service;
using Prechart.Service.Documents.Upload.Csv.Services;
using Prechart.Service.Globals.Helper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Prechart.Service.Documents.Upload.Csv.Services.DocumentsService;

namespace Prechart.Service.Documents.Upload.Csv.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/documents/")]
public class DocumentsController : ControllerBase
{
    private readonly IBatchHelper _batch;
    private readonly ILogger<DocumentsController> _logger;
    private readonly IDocumentsService _service;

    public DocumentsController(ILogger<DocumentsController> logger, IDocumentsService service, IBatchHelper batch)
    {
        _logger = logger;
        _service = service;
        _batch = batch;
    }

    [HttpPost]
    [Route("csv/upload")]
    [LogAuditAction]
    public async Task<ActionResult> UploadCsv(List<IFormFile> files)
    {
        var result = await _service.HandleAsync(new ProcessUploadedCsvFiles { Files = files }, CancellationToken.None);

        return result.ToActionResult();
    }

    [HttpPost]
    [Route("loonaangifte/upload/{xsdyear}")]
    [LogAuditAction]
    public async Task<ActionResult> UploadXml(List<IFormFile> files, int xsdyear)
    {
        var result = await _service.HandleAsync(new ProcessUploadedXmlFiles { Files = files, XsdYear = xsdyear }, CancellationToken.None);

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpPost]
    [Route("uploadpersonphoto")]
    [LogAuditAction]
    public async Task<ActionResult> UploadPersonPhoto(string personId, IFormFile personPhoto)
    {
        var results = await _service.HandleAsync(new UpdatePersonPhoto
        {
            Id = personId,
            PersonPhoto = personPhoto
        });

        return results.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpPost]
    [Route("downloadpersonphoto")]
    [LogAuditAction]
    public async Task<ActionResult> DownloadPersonPhoto(string personId)
    {
        var results = await _service.HandleAsync(new DownloadPersonPhoto
        {
            Id = personId,
        });

        if (results.IsNotFoundOrBadRequest() || results.IsFailure())
        {
            return results.ToActionResult();
        }

        return results.Value;
    }
}
