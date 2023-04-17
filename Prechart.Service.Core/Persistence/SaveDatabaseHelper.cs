using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prechart.Service.Core.Trackers;

namespace Prechart.Service.Core.Persistence;

public class SaveDatabaseHelper : ISaveDatabaseHelper
{
    private readonly ILogger<SaveDatabaseHelper> _logger;
    private readonly IEnumerable<IEntityTracker> _trackers;
    private readonly IHttpContextAccessor _httpContext;

    public SaveDatabaseHelper(ILogger<SaveDatabaseHelper> logger, IHttpContextAccessor httpContext,
        IEnumerable<IEntityTracker> trackers)
    {
        _logger = logger;
        _trackers = trackers;
        _httpContext = httpContext;
    }

    public async Task<int> SaveChangesAsync(DbContext dbContext, IEnumerable<string> trackerNames,
        CancellationToken cancellationToken = default)
    {
        var result = await dbContext.SaveChangesAsync(false, cancellationToken);

        foreach (var tracker in _trackers.Where(t => trackerNames.Contains(t.Name)))
        {
            try
            {
                tracker.SendEvents(dbContext.ChangeTracker, _httpContext?.HttpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending events");
            }
        }

        dbContext.ChangeTracker.AcceptAllChanges();

        return result;
    }
}