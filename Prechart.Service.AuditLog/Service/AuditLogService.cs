using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Models;
using Prechart.Service.AuditLog.Repositories;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Service;

public partial class AuditLogService : IAuditLogService
{
    private readonly ILogger<AuditLogService> _logger;
    private readonly IAuditLogRepository _repository;

    public AuditLogService(ILogger<AuditLogService> logger, IAuditLogRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults<bool>> HandleAsync(PostAuditLogToMongo request, CancellationToken cancellationToken)
    {
        var result = await _repository.HandleAsync(new AuditLogRepository.GetAuditLogReadyForPosting(), cancellationToken);

        if (result.IsNotFoundOrBadRequest() || result.IsFailure())
        {
            return ResultsTo.NotFound<bool>();
        }

        var forPosting = result.Value.Select(l => new MongoAuditLogs
        {
            LogDate = l.LogDate,
            User = l.User,
            Table = l.Table,
            TableId = l.TableId,
            Action = l.Action,
            Changes = l.Changes.FromJsonStringToObject<List<DataChange>>(),
            AdditionalLogs = l.AdditionalLogs,
        }).ToList();

        return await _repository.HandleAsync(new AuditLogRepository.InsertAuditLog { AuditLogs = forPosting }, cancellationToken);

    }

    public async Task<IFluentResults<bool>> HandleAsync(InsertAuditLogInMemory request, CancellationToken cancellationToken)
    {
        return await _repository.HandleAsync(new AuditLogRepository.InsertAuditLogInMemory
        {
            UserName = request.UserName,
            TableName = request.TableName,
            EntityId = request.EntityId,
            Action = request.Action,
            Changes = request.Changes,
            AdditionalLogs = null,
        }, cancellationToken);
    }

    public async Task<IFluentResults<List<Database.Models.AuditLog>>> HandleAsync(GetAuditLogReadyForPosting request, CancellationToken cancellationToken)
    {
        var result = await _repository.HandleAsync(new AuditLogRepository.GetAuditLogReadyForPosting(), cancellationToken);

        if (result.IsNotFound() || !result.Value.Any())
        {
            return ResultsTo.NotFound<List<Database.Models.AuditLog>>();
        }

        return ResultsTo.Something(result);
    }
}
