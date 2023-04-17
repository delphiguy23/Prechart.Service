using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Models.Loonheffings.Enums;

namespace Prechart.Service.Globals.Models.Xsd.Loonheffings;

public class Loonaangifte
{
    public BerichtType Bericht { get; set; }
    public LoonaangifteAdministratieveEenheid AdministratieveEenheid { get; set; }
    public LoonaangifteVersion Version { get; set; }
}

public class BerichtType
{
    public string IdBer { get; set; }
    public DateTime DatTdAanm { get; set; }
    public string ContPers { get; set; }
    public string TelNr { get; set; }
    public string RelNr { get; set; }
    public string GebrSwPakket { get; set; }
}

public class TijdvakCorrectieType
{
    public DateTime DatAanvTv { get; set; }
    public DateTime DatEindTv { get; set; }
    public CollectieveAangifteType CollectieveAangifte { get; set; }
    public InkomstenverhoudingInitieelType[] InkomstenverhoudingInitieel { get; set; }
    public InkomstenverhoudingIntrekkingType[] InkomstenverhoudingIntrekking { get; set; }
}

public class CollectieveAangifteType
{
    public string TotLnLbPh { get; set; }
    public string TotLnSv { get; set; }
    public string TotPrlnAofAnwLg { get; set; }
    public string TotPrlnAofAnwHg { get; set; }
    public string TotPrlnAofAnwUit { get; set; }
    public string TotPrlnAwfAnwLg { get; set; }
    public string TotPrlnAwfAnwHg { get; set; }
    public string TotPrlnAwfAnwHz { get; set; }
    public string PrLnUfo { get; set; }
    public string IngLbPh { get; set; }
    public string EhPubUitk { get; set; }
    public string EhGebrAuto { get; set; }
    public string Ehvut { get; set; }
    public string EhOvsFrfWrkkstrg { get; set; }
    public string AvZeev { get; set; }
    public string VrlAvso { get; set; }
    public string TotPrAofLg { get; set; }
    public string TotPrAofHg { get; set; }
    public string TotPrAofUit { get; set; }
    public string TotOpslWko { get; set; }
    public string TotPrGediffWhk { get; set; }
    public string TotPrAwfLg { get; set; }
    public string TotPrAwfHg { get; set; }
    public string TotPrAwfHz { get; set; }
    public string PrUfo { get; set; }
    public string IngBijdrZvw { get; set; }
    public string TotWghZvw { get; set; }
    public string TotTeBet { get; set; }
}

public class CollectieveAangifteTijdvakAangifteType : CollectieveAangifteType
{
    public CollectieveAangifteTijdvakAangifteTypeSaldoCorrectiesVoorgaandTijdvak[] SaldoCorrectiesVoorgaandTijdvak { get; set; }

    public string TotGen { get; set; }
}

public class CollectieveAangifteTijdvakAangifteTypeSaldoCorrectiesVoorgaandTijdvak
{
    public DateTime DatAanvTv { get; set; }
    public DateTime DatEindTv { get; set; }
    public string Saldo { get; set; }
}

public class InkomstenverhoudingInitieelType : InkomstenverhoudingType
{
    public InkomstenverhoudingInitieelTypeNatuurlijkPersoon NatuurlijkPersoon { get; set; }
    public InkomstenperiodeInitieelType[] Inkomstenperiode { get; set; }
    public WerknemersgegevensType Werknemersgegevens { get; set; }
    public SectorType[] Sector { get; set; }
}

public class InkomstenverhoudingInitieelTypeNatuurlijkPersoon : NatuurlijkPersoonType
{
    public AdresBinnenlandType AdresBinnenland { get; set; }
    public AdresBuitenlandType AdresBuitenland { get; set; }
}

public class AdresBinnenlandType : IXmlAdresBinnenland
{
    public string Str { get; set; }
    public string HuisNr { get; set; }
    public string HuisNrToev { get; set; }
    public string LocOms { get; set; }
    public string Pc { get; set; }
    public string Woonpl { get; set; }
}

public class AdresBuitenlandType : IXmlAdresBuitenland
{
    public string Str { get; set; }
    public string HuisNr { get; set; }
    public string LocOms { get; set; }
    public string Pc { get; set; }
    public string Woonpl { get; set; }
    public string Reg { get; set; }
    public string LandCd { get; set; }
}

public class NatuurlijkPersoonType
{
    public string SofiNr { get; set; }
    public string Voorl { get; set; }
    public string Voorv { get; set; }
    public string SignNm { get; set; }
    public DateTime Gebdat { get; set; }
    public bool GebdatSpecified { get; set; }
    public string Nat { get; set; }
    public Gesl Gesl { get; set; }
    public bool GeslSpecified { get; set; }
}

[BsonIgnoreExtraElements]
public class InkomstenperiodeInitieelType
{
    public DateTime DatAanv { get; set; }

    [BsonIgnore]
    [JsonConverter(typeof(XmlEnumAttribute))]
    [BsonRepresentation(BsonType.String)]
    public CdSrtIV SrtIv { get; set; }

    public string SrtIV => SrtIv.XmlEnumToString();

    [BsonIgnore]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public CdAard CdAard { get; set; }

    public string Cd_Aard => CdAard.XmlEnumToString();

    [BsonIgnore] public bool CdAardSpecified { get; set; }

    public string CdInvlVpl { get; set; }

    [BsonIgnore]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public FsIndFZ FsIndFz { get; set; }

    public string Fa_IndFz => FsIndFz.XmlEnumToString();

    [BsonIgnore] public bool FsIndFzSpecified { get; set; }

    public string Cao { get; set; }

    public string CdCaoInl { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndArbovOnbepTd { get; set; }

    [BsonIgnore] public bool IndArbovOnbepTdSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSchriftArbov { get; set; }

    [BsonIgnore] public bool IndSchriftArbovSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndOprov { get; set; }

    [BsonIgnore] public bool IndOprovSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public IndicatieJ IndJrurennrm { get; set; }

    [BsonIgnore] public bool IndJrurennrmSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndPubAanOnbepTd { get; set; }

    [BsonIgnore] public bool IndPubAanOnbepTdSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvOudrWn { get; set; }

    [BsonIgnore] public bool IndAvrLkvOudrWnSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvAgWn { get; set; }

    [BsonIgnore] public bool IndAvrLkvAgWnSpecified { get; set; }

    public Indicatie IndAvrLkvDgBafSb { get; set; }

    [BsonIgnore] public bool IndAvrLkvDgBafSbSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvHpAgWn { get; set; }

    [BsonIgnore] public bool IndAvrLkvHpAgWnSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndLhKort { get; set; }

    [BsonIgnore]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public CdRdnGnBijt CdRdnGnBijt { get; set; }

    public string Cd_RdnGnBijt => CdRdnGnBijt.XmlEnumToString();

    [BsonIgnore] public bool CdRdnGnBijtSpecified { get; set; }

    [BsonIgnore]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public LbTab LbTab { get; set; }

    public string Lb_Tab => LbTab.XmlEnumToString();

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndWao { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndWw { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndZw { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndWgldOudRegl { get; set; }

    [BsonIgnore] public bool IndWgldOudReglSpecified { get; set; }

    [BsonIgnore]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public CdZvw CdZvw { get; set; }

    public string Cd_Zvw => CdZvw.XmlEnumToString();

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndVakBn { get; set; }

    [BsonIgnore] public bool IndVakBnSpecified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSa71 { get; set; }

    public bool IndSa71Specified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSa72 { get; set; }

    [BsonIgnore] public bool IndSa72Specified { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSa03 { get; set; }

    [BsonIgnore] public bool IndSa03Specified { get; set; }

    [BsonIgnore]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public CdIncInkVerm CdIncInkVerm { get; set; }

    public string Cd_IncInkVerm => CdIncInkVerm.XmlEnumToString();


    [BsonIgnore] public bool CdIncInkVermSpecified { get; set; }
}

[BsonIgnoreExtraElements]
public class WerknemersgegevensType
{
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnLbPh { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnSv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAofAnwLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAofAnwHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAofAnwUit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAwfAnwLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAwfAnwHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAwfAnwHz { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrLnUfo { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnTabBb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VakBsl { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpgRchtVakBsl { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpnAvwb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpbAvwb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnInGld { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WrdLn { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnOwrk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VerstrAanv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal IngLbPh { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAofLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAofHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAofUit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpslWko { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrGediffWhk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAwfLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAwfHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAwfHz { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrUfo { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BijdrZvw { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WghZvw { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WrdPrGebrAut { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WrknBijdrAut { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Reisk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VerrArbKrt { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AantVerlU { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Ctrctln { get; set; }

    [BsonIgnore] public bool CtrctlnSpecified { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AantCtrcturenPWk { get; set; }

    [BsonIgnore] public bool AantCtrcturenPWkSpecified { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BedrRntKstvPersl { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BedrAlInWwb { get; set; }

    [BsonIgnore] public bool BedrAlInWwbSpecified { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BedrRchtAl { get; set; }

    [BsonIgnore] public bool BedrRchtAlSpecified { get; set; }
}

public class SectorType
{
    public DateTime DatAanvSect { get; set; }
    public DateTime DatEindSect { get; set; }
    public bool DatEindSectSpecified { get; set; }
    public string Sect { get; set; }
}

public class InkomstenverhoudingType
{
    public string NumIv { get; set; }
    public DateTime DatAanv { get; set; }
    public DateTime DatEind { get; set; }
    public bool DatEindSpecified { get; set; }
    public CdRdnEindArbov CdRdnEindArbov { get; set; }
    public bool CdRdnEindArbovSpecified { get; set; }
    public string PersNr { get; set; }
}

public class InkomstenverhoudingIntrekkingType
{
    public string NumIv { get; set; }
    public string SofiNr { get; set; }
    public string PersNr { get; set; }
}

public class TijdvakAangifteType
{
    public DateTime DatAanvTv { get; set; }
    public DateTime DatEindTv { get; set; }
    public TijdvakAangifteTypeAanvullendeAangifte AanvullendeAangifte { get; set; }
    public TijdvakAangifteTypeVolledigeAangifte VolledigeAangifte { get; set; } //
}

public class TijdvakAangifteTypeAanvullendeAangifte
{
    public CollectieveAangifteTijdvakAangifteType CollectieveAangifte { get; set; }
    public InkomstenverhoudingInitieelType[] InkomstenverhoudingInitieel { get; set; }
    public InkomstenverhoudingIntrekkingType[] InkomstenverhoudingIntrekking { get; set; }
}

public class TijdvakAangifteTypeVolledigeAangifte
{
    public CollectieveAangifteTijdvakAangifteType CollectieveAangifte { get; set; }
    public InkomstenverhoudingInitieelType[] InkomstenverhoudingInitieel { get; set; }
}

public class LoonaangifteAdministratieveEenheid
{
    public string LhNr { get; set; }
    public string NmIp { get; set; }
    public TijdvakAangifteType TijdvakAangifte { get; set; }
    public TijdvakCorrectieType TijdvakCorrectie { get; set; }
}
