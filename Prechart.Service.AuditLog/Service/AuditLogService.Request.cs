using System;
using System.Collections.Generic;
using Prechart.Service.AuditLog.Models;

namespace Prechart.Service.AuditLog.Service;

public partial class AuditLogService
{
    public record PostAuditLogToMongo;

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

    public record GetAuditLogReadyForPosting;
}
