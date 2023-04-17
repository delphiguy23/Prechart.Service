using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Interfaces.Documents;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Prechart.Service.Email.Models;

public class EmailEventRecipientModel: IEmailEventRecipientResults
{
    [JsonConverter(typeof(StringToObjectId))]
    [BsonId] public ObjectId Id { get; set; }
    public string Recipient { get; set; }
    public string Cc { get; set; }
    public string Bcc { get; set; }
    public string Name { get; set; }
    public int EmailEventType { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}


