using AutoMapper;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Repository;

namespace Prechart.Service.Klant.Test.Repository;

public partial class KlantRepositorySpec : WithSubject<KlantRepository>
{
    private List<KlantModel> _klantModelsForUpsert;
    public Mock<IFindFluent<KlantModel, KlantModel>> _mockAsyncCursorFluentKlantModel { get; set; }
    public Mock<IAsyncCursor<KlantModel>> _mockAsyncCursorKlantModel { get; set; }
    public Mock<IAsyncCursor<KlantModel>> _mockAsyncCursorKlantModelList { get; set; }

    public IMongoDbHelper _mongoDbHelper { get; set; }
    public IMapper _mapper { get; set; }
    public Mock<IMongoCollection<KlantModel>> _collection { get; set; }
    public Mock<UpdateResult> _mockUpdateResult { get; set; }
    public List<KlantModel> _klantModels { get; set; }
    public List<KlantModel> _klantModelsEmpty { get; set; }
    public List<KlantModel> _klantModelsNull { get; set; }
    public FakeLogger<KlantRepository> _logger { get; set; }


    public Given TheRepository => () =>
    {
        _logger = new FakeLogger<KlantRepository>();
        _mapper = An<IMapper>();
        _mongoDbHelper = An<IMongoDbHelper>();

        _collection = new Mock<IMongoCollection<KlantModel>>();
        _mockUpdateResult = new Mock<UpdateResult>();
        _mockAsyncCursorKlantModel = new Mock<IAsyncCursor<KlantModel>>();
        _mockAsyncCursorKlantModelList = new Mock<IAsyncCursor<KlantModel>>();
        _mockAsyncCursorFluentKlantModel = new Mock<IFindFluent<KlantModel, KlantModel>>();

        _klantModels = new List<KlantModel>
        {
            new()
            {
                Id = "633129fb572a926667ed8c5d".ToObjectId(),
                KlantNaam = "Test Klant",
                Werkgevers = new List<string> {"123456789L01"},
                ContactPersons = null,
                Active = true,
            },
        };

        _klantModelsForUpsert = new List<KlantModel>
        {
            new()
            {
                Id = "633129fb572a926667ed8c5d".ToObjectId(),
                KlantNaam = "Test Klant",
                Werkgevers = new List<string> {"123456789L01", "9876543210L01"},
                ContactPersons = null,
                Active = true,
                DateCreated = new DateTime(2022, 1, 1),
                DateLastModified = new DateTime(2022, 1, 1),
            },
        };

        _klantModelsEmpty = new List<KlantModel>();
        _klantModelsNull = null;

        Subject = new KlantRepository(_logger, _mapper, _mongoDbHelper, _collection.Object);
    };


    public bool FindKlantModel(KlantModel test)
    {
        return true;
    }
}
