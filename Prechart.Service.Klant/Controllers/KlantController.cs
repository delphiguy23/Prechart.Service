using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Models;
using Prechart.Service.Klant.Service;

namespace Prechart.Service.Klant.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/klant")]
public class KlantController : ControllerBase
{
    private readonly ILogger<KlantController> _logger;
    private readonly IKlantService _service;

    public KlantController(ILogger<KlantController> logger, IKlantService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    [Route("upsert")]
    [LogAuditAction]
    public async Task<ActionResult> AddKlant([FromBody] NewKlantModel klant)
    {
        var result = await _service.HandleAsync(new KlantService.UpsertKlants
        {
            Klants = new List<KlantModel>
            {
                new()
                {
                    KlantNaam = klant.KlantNaam,
                    ContactPersons = klant.ContactPersons,
                },
            },
        }, CancellationToken.None);

        return result.ToActionResult();
    }

    [HttpGet]
    [LogAuditAction]
    public async Task<ActionResult> GetKlants()
    {
        var result = await _service.HandleAsync(new KlantService.GetKlants(), CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("/id/{klantid}")]
    [LogAuditAction]
    public async Task<ActionResult> GetKlantById(string klantid)
    {
        var result = await _service.HandleAsync(new KlantService.GetKlant
        {
            KlantId = klantid,
        }, CancellationToken.None);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("/name/{klantname}")]
    [LogAuditAction]
    public async Task<ActionResult> GetKlantByName(string klantname)
    {
        var result = await _service.HandleAsync(new KlantService.GetKlant
        {
            KlantName = klantname,
        }, CancellationToken.None);
        return result.ToActionResult();
    }
}
