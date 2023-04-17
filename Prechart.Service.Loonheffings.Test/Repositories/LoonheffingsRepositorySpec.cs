using Bogus;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;

namespace Prechart.Service.Loonheffings.Test.Repositories;

public partial class LoonheffingsRepositorySpec : WithSubject<LoonheffingsRepository>
{
    private Mock<IAsyncCursor<UnprocessedUploads>> _mockAsyncCursorUnprocessedUploads;

    private Mock<IMongoCollection<XmlLoonaangifteUpload>> _mockCollection;
    private Mock<UpdateResult> _mockUpdateResult;
    private XmlLoonaangifteUpload taxFiling;
    private List<UnprocessedUploads> unprocessedUploadsWithCorrection;
    private List<UnprocessedUploads> unprocessedUploadsWithNormal;
    private List<UnprocessedUploads> unprocessedUploadsWithNormalAndCorrection;

    public FakeLogger<LoonheffingsRepository> _logger { get; set; }

    public Given TheRepository => () =>
    {
        var fakeData = new Faker<XmlLoonaangifteUpload>("nl")
            .RuleFor(d => d.Id, f => ObjectId.GenerateNewId())
            .RuleFor(d => d.FileName, f => f.System.FileName("xml"))
            .RuleFor(d => d.IsValid, f => true)
            .RuleFor(d => d.Errors, f => new List<string>())
            .RuleFor(d => d.EmployeesInserted, f => 0)
            .RuleFor(d => d.EmployeesUpdated, f => 0)
            .RuleFor(d => d.Processed, f => false)
            .RuleFor(d => d.ProcessedDate, f => DateTime.Now)
            .RuleFor(d => d.UploadedDate, f => DateTime.Now)
            .RuleFor(d => d.Loonaangifte, f => f.Lorem);

        unprocessedUploadsWithNormalAndCorrection = new List<UnprocessedUploads>
        {
            new()
            {
                Id = ObjectId.GenerateNewId(),
                FileName = "test.xml",
                TaxNo = "123456789L01",
                Klant = "TestKlant",
                TaxFileProcessDate = DateTime.Now,
                PeriodStart = DateTime.Now,
                PeriodEnd = DateTime.Now,
                Person = new List<NatuurlijkPersoonDetails>(),
                CollectieveAangifteNormal = new CollectieveAangifteTijdvakAangifteType
                {
                    TotGen = "0",
                },
                CollectieveAangifteCorrection = new CollectieveAangifteTijdvakAangifteType
                {
                    TotGen = "0",
                },
            },
        };

        unprocessedUploadsWithNormal = new List<UnprocessedUploads>
        {
            new()
            {
                Id = ObjectId.GenerateNewId(),
                FileName = "test.xml",
                TaxNo = "123456789L01",
                Klant = "TestKlant",
                TaxFileProcessDate = DateTime.Now,
                PeriodStart = DateTime.Now,
                PeriodEnd = DateTime.Now,
                Person = new List<NatuurlijkPersoonDetails>(),
                CollectieveAangifteNormal = new CollectieveAangifteTijdvakAangifteType
                {
                    TotGen = "0",
                },
                CollectieveAangifteCorrection = null,
            },
        };

        unprocessedUploadsWithCorrection = new List<UnprocessedUploads>
        {
            new()
            {
                Id = ObjectId.GenerateNewId(),
                FileName = "test.xml",
                TaxNo = "123456789L01",
                Klant = "TestKlant",
                TaxFileProcessDate = DateTime.Now,
                PeriodStart = DateTime.Now,
                PeriodEnd = DateTime.Now,
                Person = new List<NatuurlijkPersoonDetails>(),
                CollectieveAangifteCorrection = new CollectieveAangifteTijdvakAangifteType
                {
                    TotGen = "0",
                },
                CollectieveAangifteNormal = null,
            },
        };

        taxFiling = fakeData.Generate();

        _logger = new FakeLogger<LoonheffingsRepository>();
        _mockCollection = new Mock<IMongoCollection<XmlLoonaangifteUpload>>();
        _mockUpdateResult = new Mock<UpdateResult>();
        _mockAsyncCursorUnprocessedUploads = new Mock<IAsyncCursor<UnprocessedUploads>>();

        Subject = new LoonheffingsRepository(_logger, _mockCollection.Object);
    };
}

// public Teardown
