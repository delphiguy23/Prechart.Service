using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Models.Belastingen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/berekenen/")]
public class BelastingTabellenWitGroenController : ControllerBase
{
    private readonly ILogger<BelastingTabellenWitGroenController> _logger;
    private readonly IBelastingTabellenWitGroenService _service;
    private readonly IRequestClient<IGetWoonlandbeginselEvent> _woonlandbeginsel;

    public BelastingTabellenWitGroenController(IBelastingTabellenWitGroenService service,
        IRequestClient<IGetWoonlandbeginselEvent> Woonlandbeginsel,
        ILogger<BelastingTabellenWitGroenController> logger)
    {
        _service = service;
        _woonlandbeginsel = Woonlandbeginsel;
        _logger = logger;
    }

    [HttpPost]
    [Route("inhouding")]
    [LogAuditAction]
    public async Task<ActionResult> TaxRecords([FromBody] GetInhouding taxRecord)
    {
        var result = await _service.HandleAsync(new BelastingTabellenWitGroenService.GetInhouding
        {
            WoondlandBeginselId = taxRecord.WoondlandBeginselId,
            ProcesDatum = taxRecord.ProcesDatum,
            Loontijdvak = taxRecord.Loontijdvak,
            InkomenWit = taxRecord.InkomenWit,
            InkomenGroen = taxRecord.InkomenGroen,
            AlgemeneHeffingsKortingIndicator = taxRecord.AlgemeneHeffingsKortingIndicator,
            BasisDagen = taxRecord.BasisDagen,
            Geboortedatum = taxRecord.Geboortedatum,
        }, CancellationToken.None);

        return result.Value.Some().ToActionResult();
    }

    [HttpGet]
    [Route("allewoonlandbeginsel")]
    [LogAuditAction]
    public async Task<ActionResult> GetAlleWoonlandbeginsel()
    {
        var result = await _service.HandleAsync(new BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel(), CancellationToken.None);

        return result.Value.Some().ToActionResult();
    }

    [HttpGet]
    [Route("woonlandbeginsel/{code}")]
    [LogAuditAction]
    public async Task<ActionResult> GetWoonlandbeginsel(string code)
    {
        var WoonlandbeginselCode = await _woonlandbeginsel.GetResponse<IWoonlandbeginsel>(new GetWoonlandbeginsel { WoonlandbeginselCode = code.ToUpper() });
        return WoonlandbeginselCode.Message.Some().ToActionResult();
    }

    [HttpGet]
    [Route("loontijdvak")]
    [LogAuditAction]
    public Task<ActionResult> GetTaxPeriods()
    {
        var result = new Dictionary<int, string>();
        var taxPeriodEnums = Enum.GetValues(typeof(TaxPeriodEnum));

        foreach (var t in taxPeriodEnums)
        {
            var type = typeof(TaxPeriodEnum);
            var enumName = type.GetEnumName(t);

            if (!string.IsNullOrEmpty(enumName))
            {
                var memInfo = type.GetMember(enumName);
                var descriptionAttribute = memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                if (descriptionAttribute != null)
                {
                    result.Add((int)t, descriptionAttribute.Description);
                }
            }
        }

        return Task.FromResult(result.Values.Some().ToActionResult());
    }


    [HttpGet]
    [Route("jaar")]
    [LogAuditAction]
    public async Task<ActionResult> GetTaxYear()
    {
        var result = await _service.HandleAsync(new BelastingTabellenWitGroenService.GetTaxYear(), CancellationToken.None);

        return result.Value.Some().ToActionResult();
    }
}
