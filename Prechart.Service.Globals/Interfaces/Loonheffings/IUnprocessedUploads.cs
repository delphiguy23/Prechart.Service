using MongoDB.Bson;

namespace Prechart.Service.Globals.Interfaces.Loonheffings;

public interface IUnprocessedUploads
{
    ObjectId Id { get; set; }
    string FileName { get; set; }
    string TaxNo { get; set; }
    string Klant { get; set; }
    DateTime TaxFileProcessedDate { get; set; }
    DateTime PeriodStart { get; set; }
    DateTime PeriodEnd { get; set; }
    List<INatuurlijkPersoon> Person { get; set; }
}