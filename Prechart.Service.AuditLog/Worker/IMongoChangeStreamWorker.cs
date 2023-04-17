using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Worker;

public interface IMongoChangeStreamWorker
{
    Task DoWork(CancellationToken stoppingToken);
}
