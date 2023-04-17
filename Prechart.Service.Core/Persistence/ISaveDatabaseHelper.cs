using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Persistence;

public interface ISaveDatabaseHelper
{
    Task<int> SaveChangesAsync(DbContext dbContext, IEnumerable<string> trackerNames,
        CancellationToken cancellationToken = default);
}