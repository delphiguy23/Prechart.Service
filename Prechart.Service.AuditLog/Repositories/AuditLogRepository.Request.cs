using System;
using System.Collections.Generic;
using Prechart.Service.AuditLog.Models;
using Prechart.Service.AuditLog.Utils;

namespace Prechart.Service.AuditLog.Repositories;

public partial class AuditLogRepository : IAuditLogRepository
{
    public record InsertAuditLog
    {
        public List<MongoAuditLogs> AuditLogs { get; set; }
    }

    public record InsertAuditLogInMemory
    {
        public string UserName { get; set; }
        public DateTime Updated { get; set; }
        public string TableName { get; set; }
        public string PropertyName { get; set; }
        public int EntityId { get; set; }
        public int? EntityIdAdditional { get; set; }
        public string Action { get; set; }
        public List<DataChange> Changes { get; set; }
        public string AdditionalLogs { get; set; }
    }

    public record GetAuditTrail
    {
        public string TableName { get; set; }
        public int? TableId { get; set; }
    }

    public record LogControllerActions
    {
        public IControllerLogs ControllerLogs { get; set; }
    }

    public record GetAuditLogReadyForPosting;
}
