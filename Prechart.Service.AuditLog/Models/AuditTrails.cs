using System.Collections.Generic;
using Prechart.Service.Core.Events;

namespace Prechart.Service.AuditLog.Models;

public class AuditTrails : IAuditTrails
{
    private List<IAuditTrailEvent> _trails;

    public AuditTrails()
    {
        _trails = new List<IAuditTrailEvent>();
    }
    public List<IAuditTrailEvent> Trails
    {
        get => _trails;
        set => _trails = value;
    }
}