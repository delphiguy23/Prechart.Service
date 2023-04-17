using System.Collections.Generic;

namespace Prechart.Service.Core.Events;

public interface IAuditTrails
{
    List<IAuditTrailEvent> Trails { get; }
}