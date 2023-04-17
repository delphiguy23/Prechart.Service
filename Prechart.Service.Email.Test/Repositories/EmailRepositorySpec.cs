using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Prechart.Service.Globals.Helper;

namespace Prechart.Service.Email.Test.Repositories;

public partial class EmailRepositorySpec : WithSubject<EmailRepository>
{
    public Mock<IAsyncCursor<EmailEventRecipientModel>> _mockAsyncCursorKlantModelList { get; set; }
    private FakeLogger<EmailRepository> _logger;
    private Mock<IMongoCollection<EmailEventModel>> _mockEmailEventCollection;
    private Mock<IMongoCollection<EmailEventRecipientModel>> _mockEmailEventRecipientCollection;
    private IMongoDbHelper _mongoDbHelper;

    public Given TheRepository => () =>
    {
        _logger = new FakeLogger<EmailRepository>();

        _mockEmailEventCollection = new Mock<IMongoCollection<EmailEventModel>>();
        _mockEmailEventRecipientCollection = new Mock<IMongoCollection<EmailEventRecipientModel>>();
        _mongoDbHelper = An<IMongoDbHelper>();


        Subject = new EmailRepository(_logger, _mockEmailEventCollection.Object, _mockEmailEventRecipientCollection.Object, _mongoDbHelper);
    };
}
