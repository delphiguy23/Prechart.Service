using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Database.Context;
using Prechart.Service.Werkgever.Repository;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec : WithSubject<WerkgeverRepository>
{
    private WerkgeverDbContext _context;
    private FakeLogger<WerkgeverRepository> _logger;
    private IMapper _mapper;
    private Mock<IMongoCollection<MongoWerkgeverModel>> _mockCollection;
    private IMongoDbHelper _mongoDbHelper;

    public Given TheRepository => () =>
    {
        _logger = new FakeLogger<WerkgeverRepository>();
        _mapper = An<IMapper>();
        _mongoDbHelper = An<IMongoDbHelper>();

        var dbContext = new DbContextOptionsBuilder<WerkgeverDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new WerkgeverDbContext(dbContext);
        _mockCollection = new Mock<IMongoCollection<MongoWerkgeverModel>>();

        Subject = new WerkgeverRepository(_logger, _context, _mapper, _mockCollection.Object, _mongoDbHelper);
    };
}
