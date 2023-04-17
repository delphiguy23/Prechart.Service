using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Prechart.Service.Core.Events;
using Prechart.Service.Core.Models;

namespace Prechart.Service.Core.Trackers;

    public class AuditLogTracker : BaseEntityTracker<IEntityUpdated>
    {
        public AuditLogTracker(IPublishEndpoint publishEndpoint, ILogger<BaseEntityTracker<IEntityUpdated>> logger)
            : base(publishEndpoint, logger)
        {
        }

        public override string Name => "auditlog";

        public override IList<IEntityUpdated> GetEvents(ChangeTracker changeTracker, HttpContext httpContext) => changeTracker.Entries()
                    .Where(p => p.State == EntityState.Modified || p.State == EntityState.Added || p.State == EntityState.Deleted)
                    .Select(change => new EntityUpdated
                    {
                        Updated = DateTime.Now,
                        UserName = httpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "default user",
                        Name = change.Metadata.GetTableName(),
                        Action = change.State,
                        Ids = change.Metadata.FindPrimaryKey()?
                                 .Properties?
                                 .Select(p => change.Property(p.Name).CurrentValue)?
                                 .ToArray() ?? Array.Empty<object>(),
                        Fields = change.State == EntityState.Modified ? change.CurrentValues.Properties
                                .Where(p => change.OriginalValues[p]?.ToString() != change.CurrentValues[p]?.ToString())
                                .Select(p => new EntityUpdatedField
                                {
                                    Name = p.GetColumnName(StoreObjectIdentifier.Table(change.Metadata.GetTableName())),
                                    NewValue = change.CurrentValues[p]?.ToString() ?? string.Empty,
                                    OldValue = change.OriginalValues[p]?.ToString() ?? string.Empty,
                                }

                                as IEntityUpdatedField).ToList() : new List<IEntityUpdatedField>(),
                    }

                    as IEntityUpdated).ToList();
    }