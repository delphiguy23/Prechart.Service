using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Loonheffings.Service;

namespace Prechart.Service.Loonheffings.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/loonheffings/")]
public class LoonheffingsController : ControllerBase
{
    private readonly ILogger<LoonheffingsController> _logger;
    private readonly ILoonheffingsService _service;

    public LoonheffingsController(ILogger<LoonheffingsController> logger, ILoonheffingsService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    [Route("process")]
    [LogAuditAction]
    public async Task<ActionResult> Process()
    {
        var result = await _service.HandleAsync(new LoonheffingsService.ProcessUploadedXmls(), CancellationToken.None);
        return result.ToActionResult();
    }
}