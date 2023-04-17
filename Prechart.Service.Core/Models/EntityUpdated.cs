using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Prechart.Service.Core.Events;

namespace Prechart.Service.Core.Models;

public class EntityUpdated : IEntityUpdated
{
    public ICollection<object> Ids { get; set; }
    public DateTime Updated { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public EntityState Action { get; set; }
    public ICollection<IEntityUpdatedField> Fields { get; set; }
}

public class EntityUpdatedField : IEntityUpdatedField
{
    public string Name { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}