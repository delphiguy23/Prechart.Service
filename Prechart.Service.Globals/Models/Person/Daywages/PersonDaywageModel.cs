using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Person.Daywage;
using Prechart.Service.Globals.Models.Loonheffings;

namespace Prechart.Service.Globals.Models.Person.Daywages;

public class PersonDaywageModel : IPersonDaywage
{
    [BsonId] public ObjectId DaywageId { get; set; }
    public string TaxNo { get; set; }
    public DateTime StartOfSickLeave { get; set; }
    public DateTime StartOfRefencePeriode { get; set; }
    public DateTime EndOfReferencePeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal DaysInReferencePeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotalPaidInReferencePeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal DaywageBasedOnReferencePeriode { get; set; }

    public List<TaxPaymentDetails> TaxDetails { get; set; }

    public DateTime CalculatedOn { get; set; }

    public bool Active { get; set; }
}
