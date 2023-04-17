using MongoDB.Bson;
using Prechart.Service.Globals.Models.Werkgever;

namespace Prechart.Service.Werkgever.Service;

public partial class WerkgeverService
{
    public record GetMongoWerkgever
    {
        public string Taxno { get; set; }
    }

    public record UpsertMongoWerkgevers
    {
        public List<MongoWerkgeverModel> Werkgevers { get; set; }
    }

    public record SyncFromMongo
    {
        public List<MongoWerkgeverModel> Werkgevers { get; set; }
    }

    public record SyncFromSql;

    public record UpsertMongoWhk
    {
        public ObjectId WerkgeverId { get; set; }
        public ObjectId Id { get; set; }

        public decimal WgaVastWerkgever { get; set; }
        public decimal WgaVastWerknemer { get; set; }
        public decimal FlexWerkgever { get; set; }
        public decimal FlexWerknemer { get; set; }
        public decimal ZwFlex { get; set; }

        public DateTime ActiefVanaf { get; set; }
        public DateTime ActiefTot { get; set; }
        public int? SqlId { get; set; }

        public bool Actief { get; set; }
    }

    public record GetCollectieve
    {
        public string TaxNo { get; set; }
    }

    public record UpdateKlantWerkgever
    {
        public string KlantId { get; set; }
        public string KlantName { get; set; }
        public string TaxNo { get; set; }
    }

    public record Print
    {
        public string TaxNo { get; set; }
    }
}
