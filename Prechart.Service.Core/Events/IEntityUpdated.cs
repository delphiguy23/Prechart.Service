using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Prechart.Service.Core.Events;

public interface IEntityUpdated
{
    ICollection<object> Ids { get; }
    DateTime Updated { get; }
    string UserName { get; }
    string Name { get; }
    EntityState Action { get; }
    ICollection<IEntityUpdatedField> Fields { get; }
}

public interface IEntityUpdatedField
{
    string Name { get; }
    string OldValue { get; }
    string NewValue { get; }
}