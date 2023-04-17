using MongoDB.Driver;
using Moq;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Batch.Test.Repository;

public partial class BatchRepositorySpec : WithSubject<BatchRepository>
{
    private BatchProcess _batchRecord;
    private FakeLogger<BatchRepository> _logger;
    private Mock<IMongoCollection<BatchProcess>> _mockCollection;

    public Given TheRepository => () =>
    {
        _logger = new FakeLogger<BatchRepository>();
        _mockCollection = new Mock<IMongoCollection<BatchProcess>>();

        _batchRecord = new BatchProcess
        {
            BatchName = "TestBatch",
            TotalTask = 1,
            Status = BatchProcessStatus.ReadyForProcessing,
            PublishedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc),
            ScheduledOn = null,
            Payload = string.Empty,
        };

        Subject = new BatchRepository(_logger, _mockCollection.Object);
    };
}
