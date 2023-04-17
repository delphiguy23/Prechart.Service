using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Prechart.Service.Core.Utils;
using Newtonsoft.Json;

namespace Prechart.Service.Globals.Interfaces.Documents;


public interface IEmailEventRecipientResults
{
    [JsonConverter(typeof(StringToObjectId))]
    [BsonId] public ObjectId Id { get; set; }
    public string Recipient { get; set; }
    public string Name { get; set; }
    public int EmailEventType { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}