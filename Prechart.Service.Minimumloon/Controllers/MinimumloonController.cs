using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Minimumloon.Database.Models;
using Prechart.Service.Minimumloon.Models;
using Prechart.Service.Minimumloon.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/minimumloon/")]
public class MinimumloonController : ControllerBase
{
    private readonly ILogger<MinimumloonController> _logger;
    private readonly IMinimumloonService _service;

    public MinimumloonController(
        IMinimumloonService service, ILogger<MinimumloonController> logger)
    {
        _service = service;
        _logger = logger;
        _logger = logger;
    }

    [HttpPost]
    [Route("upsert")]
    [LogAuditAction]
    public async Task<IActionResult> UpsertMinimumLoon(List<MinimumloonModel> loons)
    {
        _logger.LogInformation("Upserting Minimumloons");

        var result = await _service.HandleAsync(new MinimumloonService.UpsertMinimumLoon
        {
            Minimumloon = loons,
        }, CancellationToken.None);

        return result.ToActionResult();
    }

    [HttpGet]
    [Route("getminimumloon/{datum}")]
    [LogAuditAction]
    public async Task<IActionResult> GetMinimumloonPerYear(DateTime datum)
    {
        var result = await _service.HandleAsync(new MinimumloonService.GetMinimumloon { Datum = datum }, CancellationToken.None);

        return result.ToActionResult();
    }

    [HttpPost]
    [Route("delete")]
    [LogAuditAction]
    public async Task<IActionResult> DeactivateMinimumloon(DeleteMinimumLoonModel request)
    {
        var result = await _service.HandleAsync(new MinimumloonService.DeleteMinimumloon { Datum = request.Datum, Leeftijd = request.Leeftijd }, CancellationToken.None);

        return result.ToActionResult();
    }
}
