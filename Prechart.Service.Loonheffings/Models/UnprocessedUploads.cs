using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;

namespace Prechart.Service.Loonheffings.Models;

[BsonIgnoreExtraElements]
public class UnprocessedUploads
{
    [BsonId] public ObjectId Id { get; set; }

    public string FileName { get; set; }
    public string TaxNo { get; set; }
    public string Klant { get; set; }
    public DateTime TaxFileProcessDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public List<NatuurlijkPersoonDetails> Person { get; set; }
    public CollectieveAangifteTijdvakAangifteType CollectieveAangifteNormal { get; set; }
    public CollectieveAangifteTijdvakAangifteType CollectieveAangifteCorrection { get; set; }
}
