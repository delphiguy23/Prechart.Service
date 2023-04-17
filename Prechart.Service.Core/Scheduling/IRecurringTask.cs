using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Core.Scheduling;

public interface IRecurringTask<T> : IInitializable, IRecurringTask
    where T : IRecurringTask
{
}

public interface IRecurringTask
{
    string Schedule { get; }

    string Name { get; }

    [DisableConcurrentExecution(10)]
    [AutomaticRetry(Attempts = 0, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    Task Handle(PerformContext context);
}
