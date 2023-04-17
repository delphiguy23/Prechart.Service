using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Models.Xsd.Loonheffings;

namespace Prechart.Service.Loonheffings.Models;

public class TaxFiling
{
    public TaxFiling(string filename)
    {
        FileName = filename;
    }

    [BsonId] public ObjectId Id { get; set; }
    public string FileName { get; set; }
    public Loonaangifte Loonaangifte { get; set; }
    public DateTime UploadDate = DateTime.Today;
    public string XmlContent { get; set; }
}