using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;

namespace Prechart.Service.Core.Scheduling;

public abstract class RecurringTask<T> : IRecurringTask<T>
    where T : IRecurringTask
{
    public Task Init()
    {
        RecurringJob.AddOrUpdate<IRecurringTask<T>>(Name, a => a.Handle(null), Schedule, TimeZoneInfo.Local);
        return Task.CompletedTask;
    }

    public abstract string Schedule { get; }
    public abstract string Name { get; }

    public abstract Task Handle(PerformContext context);
}
