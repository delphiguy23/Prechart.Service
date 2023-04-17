using Prechart.Service.AuditLog.Models;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System.Collections.Generic;

namespace Prechart.Service.AuditLog.Repositories;

public interface IAuditLogRepository :
    IHandlerAsync<AuditLogRepository.InsertAuditLog, IFluentResults<bool>>,
    IHandlerAsync<AuditLogRepository.LogControllerActions, IFluentResults<bool>>,
    IHandlerAsync<AuditLogRepository.GetAuditTrail, IFluentResults<List<MongoAuditLogs>>>,
    IHandlerAsync<AuditLogRepository.GetAuditLogReadyForPosting, IFluentResults<List<Database.Models.AuditLog>>>,
    IHandlerAsync<AuditLogRepository.InsertAuditLogInMemory, IFluentResults<bool>>
{
}
