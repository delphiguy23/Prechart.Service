using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Prechart.Service.Core.Trackers;

public interface IEntityTracker
    {
        void SendEvents(ChangeTracker changeTracker, HttpContext httpContext);

        string Name { get; }
    }
