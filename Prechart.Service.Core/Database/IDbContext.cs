using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Database
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
