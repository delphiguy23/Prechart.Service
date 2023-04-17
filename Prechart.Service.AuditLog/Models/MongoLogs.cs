using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.AuditLog.Models;

public class MongoLogs
{
    [BsonId] public ObjectId Id { get; set; }

    public DateTime ChangeDateTime { get; set; }
    public string Database { get; set; }
    public string Collection { get; set; }
    public BsonDocument CollectionDocumentId { get; set; }
    public string OperationType { get; set; }
    public BsonDocument UpdateDescription { get; set; }
}
