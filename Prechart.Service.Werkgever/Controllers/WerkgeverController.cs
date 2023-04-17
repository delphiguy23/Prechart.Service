using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Models;
using Prechart.Service.Werkgever.Service;

namespace Prechart.Service.Werkgever.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/werkgever")]
public class WerkgeverController : ControllerBase
{
    private readonly ILogger<WerkgeverController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IWerkgeverService _service;

    public WerkgeverController(ILogger<WerkgeverController> logger, IWerkgeverService service,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _service = service;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Route("{taxno}")]
    [LogAuditAction]
    public async Task<ActionResult> GetMongoWerkgever(string taxno)
    {
        if (await _service.HandleAsync(new WerkgeverService.GetMongoWerkgever {Taxno = taxno}, CancellationToken.None) is { } result)
        {
            return result.ToActionResult(result.Value.FirstOrDefault());
        }

        return NotFound();
    }

    [HttpGet]
    [Route("all")]
    [LogAuditAction]
    public async Task<ActionResult> GetMongoWerkgevers()
    {
        if (await _service.HandleAsync(new WerkgeverService.GetMongoWerkgever(), CancellationToken.None) is { } result)
        {
            return result.ToActionResult(result.Value);
        }

        return NotFound();
    }

    [HttpPost]
    [Route("upsert")]
    [LogAuditAction]
    public async Task<ActionResult> UpsertMongoWerkgeverAndWhk([FromBody] UpsertMongoWerkgeverAndWhk request)
    {
        var result = await _service.HandleAsync(new WerkgeverService.UpsertMongoWerkgevers
        {
            Werkgevers = new List<MongoWerkgeverModel>
            {
                request.Werkgever,
            },
        }, CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("upsert/werkgever")]
    [LogAuditAction]
    public async Task<ActionResult> UpsertMongoWerkgever([FromBody] UpsertMongoWerkgever request)
    {
        var result = await _service.HandleAsync(new WerkgeverService.UpsertMongoWerkgevers
        {
            Werkgevers = new List<MongoWerkgeverModel>
            {
                new()
                {
                    Id = request.Id,
                    Klant = request.Klant,
                    Naam = request.Naam,
                    Sector = request.Sector,
                    FiscaalNummer = request.FiscaalNummer,
                    LoonheffingenExtentie = request.LoonheffingenExtentie,
                    OmzetbelastingExtentie = request.OmzetbelastingExtentie,
                    DatumActiefVanaf = request.DatumActiefVanaf,
                    DatumActiefTot = request.DatumActiefTot,
                    Actief = request.Actief,
                },
            },
        }, CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("upsert/whk")]
    [LogAuditAction]
    public async Task<ActionResult> UpsertMongoWhk([FromBody] UpsertMongoWhk request)
    {
        var result = await _service.HandleAsync(new WerkgeverService.UpsertMongoWhk
        {
            WerkgeverId = request.WerkgeverId,
            Id = request.Id ?? ObjectId.GenerateNewId(),
            WgaVastWerkgever = request.WgaVastWerkgever,
            WgaVastWerknemer = request.WgaVastWerknemer,
            FlexWerkgever = request.FlexWerkgever,
            FlexWerknemer = request.FlexWerknemer,
            ZwFlex = request.ZwFlex,
            ActiefVanaf = request.ActiefVanaf,
            ActiefTot = request.ActiefTot,
            SqlId = request.SqlId,
            Actief = request.Actief,
        }, CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("sync")]
    [LogAuditAction]
    public async Task<ActionResult> SyncFromSql()
    {
        var result = await _service.HandleAsync(new WerkgeverService.SyncFromSql(), CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("collectieve/{taxno}")]
    [LogAuditAction]
    public async Task<ActionResult> GetCollectieve(string taxno)
    {
        return (await _service.HandleAsync(new WerkgeverService.GetCollectieve
        {
            TaxNo = taxno,
        }, CancellationToken.None)).ToActionResult();
    }

    [HttpGet]
    [Route("print/{taxno}")]
    [LogAuditAction]
    public async Task<ActionResult> Print(string taxno)
    {
        return (await _service.HandleAsync(new WerkgeverService.Print {TaxNo = taxno})).ToActionResult();
    }
}
