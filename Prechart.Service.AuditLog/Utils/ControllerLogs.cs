using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Prechart.Service.AuditLog.Utils;

public class ControllerLogs : IControllerLogs
{
    public ObjectId Id { get; set; }
    public string User { get; set; }
    public DateTime ActionOn { get; set; }

    public List<Dictionary<string, object>> Data { get; set; }
}
