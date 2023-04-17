using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.Service;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route("/platform/service/api/premiebedrag/")]
public class PremiePercentagesSocialeVerzekeringenController : ControllerBase
{
    private readonly ILogger<PremiePercentagesSocialeVerzekeringenController> _logger;
    private readonly IPremiePercentagesSocialeVerzekeringenService _service;

    public PremiePercentagesSocialeVerzekeringenController(IPremiePercentagesSocialeVerzekeringenService service, ILogger<PremiePercentagesSocialeVerzekeringenController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> GetPremieBedrag([FromBody] GetPremieBedrag request)
    {
        var result = await _service.HandleAsync(new PremiePercentagesSocialeVerzekeringenService.GetPremiePercentage
        {
            LoonSocialVerzekeringen = request.LoonSocialVerzekeringen,
            LoonZiektekostenVerzekeringsWet = request.LoonZiektekostenVerzekeringsWet,
            SocialeVerzekeringenDatum = request.SocialeVerzekeringenDatum,
        }, CancellationToken.None);

        return result.Some().ToActionResult();
    }
}
