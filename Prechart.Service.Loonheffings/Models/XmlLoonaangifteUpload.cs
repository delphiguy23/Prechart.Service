using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Loonheffings;

namespace Prechart.Service.Loonheffings.Models;

[BsonIgnoreExtraElements]
public class XmlLoonaangifteUpload : IXmlLoonaangifteUpload
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string FileName { get; set; }
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
    public object Loonaangifte { get; set; }
    public int EmployeesInserted { get; set; }
    public int EmployeesUpdated { get; set; }
    public bool Processed { get; set; }
    public DateTime UploadedDate { get; set; }
    public DateTime ProcessedDate { get; set; }
}