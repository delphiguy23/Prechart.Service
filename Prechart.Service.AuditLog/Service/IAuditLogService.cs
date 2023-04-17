using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System.Collections.Generic;

namespace Prechart.Service.AuditLog.Service;

public interface IAuditLogService :
    IHandlerAsync<AuditLogService.PostAuditLogToMongo, IFluentResults<bool>>,
    IHandlerAsync<AuditLogService.InsertAuditLogInMemory, IFluentResults<bool>>,
    IHandlerAsync<AuditLogService.GetAuditLogReadyForPosting, IFluentResults<List<Database.Models.AuditLog>>>
{
}
