using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Belastingen.Models.Berekeningen;
using Prechart.Service.Belastingen.Services.Berekeningen;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/berekeningen/")]
public class BerekeningenController : ControllerBase
{
    private readonly IRequestClient<IMongoGetWerkgever> _getWerkgeverEvent;
    private readonly ILogger<BerekeningenController> _logger;
    private readonly IBerekeningenService _service;

    public BerekeningenController(ILogger<BerekeningenController> logger, IBerekeningenService service, IRequestClient<IMongoGetWerkgever> getWerkgeverEvent)
    {
        _logger = logger;
        _service = service;
        _getWerkgeverEvent = getWerkgeverEvent;
    }

    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> GetBerekeningen([FromBody] GetBerekenen request)
    {
        var result = (await _service.HandleAsync(new BerekeningenService.CalculateBerekenen { Parameters = request }, CancellationToken.None));
        return result.Value.Some().ToActionResult();
    }

    [HttpGet]
    [LogAuditAction]
    public async Task<ActionResult> GetWerkgever()
    {
        var result = await _getWerkgeverEvent.GetResponse<IMongoWerkgevers, NotFound>(new GetWhkWerkgeverEventModel());

        if (result.Is(out Response<IMongoWerkgevers> werkgevers))
        {
            return werkgevers.Message.Some().ToActionResult();
        }

        return NotFound();
    }
}
