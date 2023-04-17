using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Person.Daywage;

namespace Prechart.Service.Globals.Models.Person.Daywages;

public class PersonDaywageHistoryModel : IPersonDaywageHistory
{
    [BsonId] public ObjectId DaywageId { get; set; }
    public string TaxNo { get; set; }
    public DateTime StartOfSickLeave { get; set; }
    public DateTime StartOfReferencePeriode { get; set; }
    public DateTime EndOfReferencePeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal DaysInReferencePeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotalPaidInReferencePeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal DaywageBasedOnReferencePeriode { get; set; }

    public List<TaxPaymentDaywageDetails> TaxDetails { get; set; }
    public DateTime CalculatedOn { get; set; }

    public bool Active { get; set; }
}
