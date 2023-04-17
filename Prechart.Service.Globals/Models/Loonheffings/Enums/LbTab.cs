using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum LbTab
{
    /// <remarks />
    [XmlEnum("010")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item010,

    /// <remarks />
    [XmlEnum("011")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item011,

    /// <remarks />
    [XmlEnum("012")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item012,

    /// <remarks />
    [XmlEnum("013")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item013,

    /// <remarks />
    [XmlEnum("014")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item014,

    /// <remarks />
    [XmlEnum("015")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item015,

    /// <remarks />
    [XmlEnum("020")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item020,

    /// <remarks />
    [XmlEnum("021")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item021,

    /// <remarks />
    [XmlEnum("022")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item022,

    /// <remarks />
    [XmlEnum("023")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item023,

    /// <remarks />
    [XmlEnum("024")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item024,

    /// <remarks />
    [XmlEnum("025")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item025,

    /// <remarks />
    [XmlEnum("210")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item210,

    /// <remarks />
    [XmlEnum("220")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item220,

    /// <remarks />
    [XmlEnum("221")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item221,

    /// <remarks />
    [XmlEnum("224")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item224,

    /// <remarks />
    [XmlEnum("225")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item225,

    /// <remarks />
    [XmlEnum("226")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item226,

    /// <remarks />
    [XmlEnum("227")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item227,

    /// <remarks />
    [XmlEnum("228")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item228,

    /// <remarks />
    [XmlEnum("250")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item250,

    /// <remarks />
    [XmlEnum("310")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item310,

    /// <remarks />
    [XmlEnum("311")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item311,

    /// <remarks />
    [XmlEnum("312")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item312,

    /// <remarks />
    [XmlEnum("313")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item313,

    /// <remarks />
    [XmlEnum("314")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item314,

    /// <remarks />
    [XmlEnum("315")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item315,

    /// <remarks />
    [XmlEnum("320")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item320,

    /// <remarks />
    [XmlEnum("321")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item321,

    /// <remarks />
    [XmlEnum("322")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item322,

    /// <remarks />
    [XmlEnum("323")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item323,

    /// <remarks />
    [XmlEnum("324")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item324,

    /// <remarks />
    [XmlEnum("325")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item325,

    /// <remarks />
    [XmlEnum("510")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item510,

    /// <remarks />
    [XmlEnum("511")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item511,

    /// <remarks />
    [XmlEnum("512")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item512,

    /// <remarks />
    [XmlEnum("513")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item513,

    /// <remarks />
    [XmlEnum("514")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item514,

    /// <remarks />
    [XmlEnum("515")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item515,

    /// <remarks />
    [XmlEnum("520")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item520,

    /// <remarks />
    [XmlEnum("521")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item521,

    /// <remarks />
    [XmlEnum("522")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item522,

    /// <remarks />
    [XmlEnum("523")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item523,

    /// <remarks />
    [XmlEnum("524")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item524,

    /// <remarks />
    [XmlEnum("525")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item525,

    /// <remarks />
    [XmlEnum("610")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item610,

    /// <remarks />
    [XmlEnum("611")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item611,

    /// <remarks />
    [XmlEnum("612")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item612,

    /// <remarks />
    [XmlEnum("613")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item613,

    /// <remarks />
    [XmlEnum("614")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item614,

    /// <remarks />
    [XmlEnum("615")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item615,

    /// <remarks />
    [XmlEnum("620")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item620,

    /// <remarks />
    [XmlEnum("621")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item621,

    /// <remarks />
    [XmlEnum("622")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item622,

    /// <remarks />
    [XmlEnum("623")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item623,

    /// <remarks />
    [XmlEnum("624")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item624,

    /// <remarks />
    [XmlEnum("625")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item625,

    /// <remarks />
    [XmlEnum("710")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item710,

    /// <remarks />
    [XmlEnum("711")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item711,

    /// <remarks />
    [XmlEnum("712")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item712,

    /// <remarks />
    [XmlEnum("713")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item713,

    /// <remarks />
    [XmlEnum("714")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item714,

    /// <remarks />
    [XmlEnum("715")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item715,

    /// <remarks />
    [XmlEnum("720")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item720,

    /// <remarks />
    [XmlEnum("721")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item721,

    /// <remarks />
    [XmlEnum("722")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item722,

    /// <remarks />
    [XmlEnum("723")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item723,

    /// <remarks />
    [XmlEnum("724")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item724,

    /// <remarks />
    [XmlEnum("725")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item725,

    /// <remarks />
    [XmlEnum("940")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item940,

    /// <remarks />
    [XmlEnum("950")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item950,

    /// <remarks />
    [XmlEnum("999")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item999,
}
