using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;
using Prechart.Service.Email.Models;

namespace Prechart.Service.Email.Repositories;

public partial class EmailRepository
{
    public record GetEmailEventType
    {
        public int EmailEventType { get; set; }
    }

    public record InsertEmailEvent
    {
        public EmailEventModel EmailEvent { get; set; }
    }

    public record UpdateEmailEvent
    {
        [JsonConverter(typeof(StringToObjectId))]
        [BsonId] public ObjectId Id { get; set; }
        public string? Error { get; set; }
        public bool Sent { get; set; }
    }

    public record UpsertEmailEventRecipient
    {
        public EmailEventRecipientModel EmailEvent { get; set; }
    }

    public record GetPendingEmails
    {
        public bool PendingEmails { get; set; }
    }
}
