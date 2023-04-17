using System.Text;
using AutoMapper;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Interfaces.Documents;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;
using Prechart.Service.Loonheffings.Service;

namespace Prechart.Service.Loonheffings.Test.Service;

public partial class LoonheffingsServiceSpec : WithSubject<LoonheffingsService>
{
    private IBatchHelper _batchHelper;
    private List<NatuurlijkPersoonDetails> _person;
    private UnprocessedUploads _unprocessedUploads;
    private UpdateResult _updateResultFound;
    private UpdateResult _updateResultNotFound;
    private string _xmlData;
    private XmlStreamData _xmlDataBytes;
    private ILoonheffingsRepository _repository { get; set; }
    private IMapper _mapper { get; set; }
    private IXsdHelper _helper { get; set; }
    private IPublishEndpoint _publishEndpoint { get; set; }
    private FakeLogger<LoonheffingsService> _logger { get; set; }

    public Given TheService => () =>
    {
        _logger = new FakeLogger<LoonheffingsService>();
        _publishEndpoint = An<IPublishEndpoint>();
        _helper = An<IXsdHelper>();
        _mapper = An<IMapper>();
        _repository = An<ILoonheffingsRepository>();

        _updateResultFound = new UpdateResult.Acknowledged(1, 1, ObjectId.GenerateNewId());
        _xmlData =
            "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><Loonaangifte version=\"2.0\" xmlns=\"http://xml.belastingdienst.nl/schemas/Loonaangifte/2022/01\"><Bericht><IdBer>00/001224153L05/00</IdBer><DatTdAanm>2022-07-26T02:28:00</DatTdAanm><ContPers>Rogier Paulissen</ContPers><TelNr>0248909470</TelNr><RelNr>SWO02310</RelNr><GebrSwPakket>Klout7.nl</GebrSwPakket></Bericht><AdministratieveEenheid><LhNr>001224153L05</LhNr><NmIP>Klant_60116</NmIP><TijdvakAangifte><DatAanvTv>2022-03-01</DatAanvTv><DatEindTv>2022-03-31</DatEindTv><VolledigeAangifte><CollectieveAangifte><TotLnLbPh>0</TotLnLbPh><TotLnSV>0</TotLnSV><TotPrlnAofAnwLg>0</TotPrlnAofAnwLg><TotPrlnAofAnwHg>0</TotPrlnAofAnwHg><TotPrlnAofAnwUit>0</TotPrlnAofAnwUit><TotPrlnAwfAnwLg>0</TotPrlnAwfAnwLg><TotPrlnAwfAnwHg>0</TotPrlnAwfAnwHg><TotPrlnAwfAnwHz>0</TotPrlnAwfAnwHz><PrLnUFO>0</PrLnUFO><IngLbPh>0</IngLbPh><TotPrAwfLg>0</TotPrAwfLg><IngBijdrZvw>0</IngBijdrZvw><TotWghZvw>0</TotWghZvw><TotTeBet>0</TotTeBet><TotGen>0</TotGen></CollectieveAangifte></VolledigeAangifte></TijdvakAangifte></AdministratieveEenheid></Loonaangifte>";
        _xmlDataBytes = new XmlStreamData
        {
            FileName = "test.xml",
            Stream = Encoding.ASCII.GetBytes(_xmlData),
        };

        _person = new List<NatuurlijkPersoonDetails>
        {
            new()
            {
                NumIV = "0000",
                PersNr = "1111",
                NatuurlijkPersoon = new InkomstenverhoudingInitieelTypeNatuurlijkPersoon
                {
                    SofiNr = "123456789",
                },
                Werknemersgegevens = new WerknemersgegevensType
                {
                    LnLbPh = 1.0M,
                },
                Inkomstenperiode = new List<InkomstenperiodeInitieelType>
                {
                    new()
                    {
                        DatAanv = default,
                        SrtIV = CdSrtIV.Item11,
                        CdAard = CdAard.Item1,
                        CdAardSpecified = false,
                        CdInvlVpl = null,
                        FsIndFZ = FsIndFZ.Item1,
                        FsIndFZSpecified = false,
                        CAO = null,
                        CdCaoInl = null,
                        IndArbovOnbepTd = Indicatie.J,
                        IndArbovOnbepTdSpecified = false,
                        IndSchriftArbov = Indicatie.J,
                        IndSchriftArbovSpecified = false,
                        IndOprov = Indicatie.J,
                        IndOprovSpecified = false,
                        IndJrurennrm = IndicatieJ.J,
                        IndJrurennrmSpecified = false,
                        IndPubAanOnbepTd = Indicatie.J,
                        IndPubAanOnbepTdSpecified = false,
                        IndAvrLkvOudrWn = Indicatie.J,
                        IndAvrLkvOudrWnSpecified = false,
                        IndAvrLkvAgWn = Indicatie.J,
                        IndAvrLkvAgWnSpecified = false,
                        IndAvrLkvDgBafSb = Indicatie.J,
                        IndAvrLkvDgBafSbSpecified = false,
                        IndAvrLkvHpAgWn = Indicatie.J,
                        IndAvrLkvHpAgWnSpecified = false,
                        IndLhKort = Indicatie.J,
                        CdRdnGnBijt = CdRdnGnBijt.Item1,
                        CdRdnGnBijtSpecified = false,
                        LbTab = LbTab.Item010,
                        IndWAO = Indicatie.J,
                        IndWW = Indicatie.J,
                        IndZW = Indicatie.J,
                        IndWgldOudRegl = Indicatie.J,
                        IndWgldOudReglSpecified = false,
                        CdZvw = CdZvw.A,
                        IndVakBn = Indicatie.J,
                        IndVakBnSpecified = false,
                        IndSA71 = Indicatie.J,
                        IndSA71Specified = false,
                        IndSA72 = Indicatie.J,
                        IndSA72Specified = false,
                        IndSA03 = Indicatie.J,
                        IndSA03Specified = false,
                        CdIncInkVerm = CdIncInkVerm.K,
                        CdIncInkVermSpecified = false,
                    },
                },
            },
        };

        _unprocessedUploads = new UnprocessedUploads
        {
            Id = ObjectId.GenerateNewId(),
            FileName = "test.xml",
            TaxNo = "123456789L01",
            Klant = "test klant",
            TaxFileProcessDate = DateTime.Now,
            PeriodStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
            PeriodEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
            Person = _person,
            CollectieveAangifteNormal = null,
            CollectieveAangifteCorrection = null,
        };

        _batchHelper = An<IBatchHelper>();

        Subject = new LoonheffingsService(_logger, _publishEndpoint, _helper, _mapper, _repository);
    };
}

public class XmlStreamData : IXmlStream
{
    public string FileName { get; set; }
    public byte[] Stream { get; set; }
}
