using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.AuditLog.Models;

public class MongoAuditLogs
{
    [BsonId] public ObjectId Id { get; set; }
    public DateTime LogDate { get; set; }
    public string User { get; set; }
    public string Table { get; set; }
    public int TableId { get; set; }
    public string Action { get; set; }
    public List<DataChange> Changes { get; set; }
    public string AdditionalLogs { get; set; }
}
