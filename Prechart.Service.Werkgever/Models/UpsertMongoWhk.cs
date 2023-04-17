using MongoDB.Bson;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;

namespace Prechart.Service.Werkgever.Models;

public class UpsertMongoWhk
{
    [JsonConverter(typeof(StringToObjectId))]
    public ObjectId WerkgeverId { get; set; }

    [JsonConverter(typeof(StringToObjectId))]
    public ObjectId? Id { get; set; }

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
