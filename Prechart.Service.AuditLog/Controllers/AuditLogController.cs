using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Repositories;
using Prechart.Service.AuditLog.Service;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Controllers;

[Authorize]
[ApiController]
[Route("/platform/service/api/auditlog")]
public class AuditLogController
{
    private readonly ILogger<AuditLogController> _logger;
    private readonly IAuditLogRepository _repository;
    private readonly IAuditLogService _service;

    public AuditLogController(ILogger<AuditLogController> logger, IAuditLogService service, IAuditLogRepository repository)
    {
        _logger = logger;
        _service = service;
        _repository = repository;
    }

    [HttpGet]
    [Route("{tablename}/{tableid?}")]
    public async Task<ActionResult> Get(string tablename, int? tableid = 0)
    {
        var trail = await _repository.HandleAsync(new AuditLogRepository.GetAuditTrail
        {
            TableName = tablename,
            TableId = tableid,
        }, CancellationToken.None);

        return trail.ToActionResult();
    }

    [HttpGet]
    [Route("forposting")]
    public async Task<ActionResult> ForPosting()
    {
        var result = await _service.HandleAsync(new AuditLogService.GetAuditLogReadyForPosting(), CancellationToken.None);

        return result.ToActionResult();
    }
}
