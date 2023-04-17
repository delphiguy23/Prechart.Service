using Prechart.Service.Batch.Repositories;
using Prechart.Service.Batch.Services;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Klant.Test.Service;

public partial class BatchServiceSpec : WithSubject<BatchService>
{
    private IBatchHelper _batchHelper;
    private BatchProcess _batchRecord;
    private FakeLogger<BatchService> _logger;
    private IBatchRepository _repository;

    public Given TheService => () =>
    {
        _logger = new FakeLogger<BatchService>();
        _repository = An<IBatchRepository>();
        _batchHelper = An<IBatchHelper>();

        _batchRecord = new BatchProcess
        {
            BatchName = "TestBatch",
            TotalTask = 1,
            Status = BatchProcessStatus.ReadyForProcessing,
            PublishedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc),
            ScheduledOn = null,
            Payload = string.Empty,
        };

        Subject = new BatchService(_logger, _repository, _batchHelper);
    };
}
