using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Prechart.Service.Core.Events;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Utils;
using Newtonsoft.Json;
using Prechart.Service.Globals.Models.Email;

namespace Prechart.Service.Email.Models;

public class EmailEventModel 
{
    [JsonConverter(typeof(StringToObjectId))]
    [BsonId] public ObjectId Id { get; set; }
    public string EmailFrom { get; set; }
    public string Sender { get; set; }
    public List<string> To { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<EmailAttachmentModel> Attachments { get; set; }
    public EmailEventType EmailEventType { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public DateTime Created { get; set; }
    public string Error { get; set; }
    public bool Sent { get; set; }

}