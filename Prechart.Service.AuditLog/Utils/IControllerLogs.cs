using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Prechart.Service.AuditLog.Utils;

public interface IControllerLogs
{
    ObjectId Id { get; set; }
    string User { get; set; }
    DateTime ActionOn { get; set; }

    List<Dictionary<string, object>> Data { get; set; }
}
