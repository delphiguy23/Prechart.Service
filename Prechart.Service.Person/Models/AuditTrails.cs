using System.Collections.Generic;
using Prechart.Service.Core.Events;

namespace Prechart.Service.Person.Models;

public class AuditTrails : IAuditTrails
{
    public List<IAuditTrailEvent> Trails { get; set; }
}