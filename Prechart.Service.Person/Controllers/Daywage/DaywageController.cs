using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Person.Services.Daywage;

namespace Prechart.Service.Person.Controllers.Daywage;

[ApiController]
[Route("/platform/service/api/daywage/")]
public class DaywageController : ControllerBase
{
    private readonly IDaywageService _service;

    public DaywageController(IDaywageService service)
    {
        _service = service;
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("getreferenceperiode/{startOfSickLeave}")]
    [LogAuditAction]
    public async Task<ActionResult> GetReferencePeriode(DateTime startOfSickLeave)
    {
        var result = await _service.HandleAsync(new DaywageService.GetReferencePeriod
        {
            StartOfSickLeave = startOfSickLeave,
        });

        if (result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPost]
    [Route("calculate")]
    [LogAuditAction]
    public async Task<ActionResult> CalculateDaywage([FromBody] DaywageService.CalculateWithInReferencePeriod request)
    {
        var result = await _service.HandleAsync(request);

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpGet]
    [Route("{personId}/{taxnumber}")]
    [LogAuditAction]
    public async Task<ActionResult> GetDayWage(string personId, string taxnumber)
    {
        var result = await _service.HandleAsync(new DaywageService.GetDaywage
        {
            PersonId = personId,
            TaxNumber = taxnumber,
        });

        return result.ToActionResult();
    }
}
