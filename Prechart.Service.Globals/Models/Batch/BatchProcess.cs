using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Globals.Models.Batch;

public class BatchProcess : IBatchProcessModel
{
    [BsonId]
    [JsonConverter(typeof(StringToObjectId))]
    public ObjectId Id { get; set; }

    public string BatchName { get; set; }
    public int TotalTask { get; set; }
    public int CompletedTask { get; set; }
    public BatchProcessStatus Status { get; set; }
    public DateTime PublishedOn { get; set; }
    public DateTime? ScheduledOn { get; set; }
    public DateTime? StartedOn { get; set; }
    public DateTime? FinalizedOn { get; set; }
    public string Payload { get; set; }
    public List<string> BatchErrors { get; set; }
}
