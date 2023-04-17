using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Person.Daywage;
using Prechart.Service.Globals.Models.Loonheffings;

namespace Prechart.Service.Person.Models.Daywage;

public class DaywageWithInReferencePeriodeRecord : IPersonDaywage
{
    [BsonId] public ObjectId DaywageId { get; set; }
    public string TaxNo { get; set; }
    public DateTime StartOfSickLeave { get; set; }
    public DateTime StartOfRefencePeriode { get; set; }
    public DateTime EndOfReferencePeriode { get; set; }
    public decimal DaysInReferencePeriode { get; set; }
    public decimal TotalPaidInReferencePeriode { get; set; }
    public decimal DaywageBasedOnReferencePeriode { get; set; }
    public List<TaxPaymentDetails> TaxDetails { get; set; }
    public DateTime CalculatedOn { get; set; }
    public bool Active { get; set; }
}
