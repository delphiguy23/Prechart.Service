using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Worker;

namespace Prechart.Service.AuditLog.Service;

public class MongoChangeStreamService : BackgroundService
{
    private readonly ILogger<MongoChangeStreamService> _logger;
    private readonly IMongoChangeStreamWorker _worker;

    public MongoChangeStreamService(ILogger<MongoChangeStreamService> logger,
        IMongoChangeStreamWorker worker)
    {
        _logger = logger;
        _worker = worker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _worker.DoWork(stoppingToken);
        }
    }
}
