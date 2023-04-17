using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Models;
using Prechart.Service.AuditLog.Service;
using Prechart.Service.Core.Events;
using Prechart.Service.Core.Models;

namespace Prechart.Service.AuditLog.Consumers;

public class EntityUpdatedConsumer : IConsumer<IEntityUpdated>
{
    private readonly ILogger<EntityUpdatedConsumer> _logger;
    private readonly IAuditLogService _service;

    public EntityUpdatedConsumer(ILogger<EntityUpdatedConsumer> logger, IAuditLogService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IEntityUpdated> context)
    {
        try
        {
            var emptyField = new EntityUpdatedField {Name = string.Empty};

            var fields = (context.Message?.Fields?.Count ?? 0) > 0 ? context.Message.Fields : new[] {emptyField};

            var ids = context.Message.Ids.SelectMany(i => GetIntValue(i)).ToList();

            if (ids.Any())
            {
                if (ids.Any(i => i.Converted))
                {
                    if (context.Message.Action == EntityState.Added)
                    {
                        emptyField.NewValue = string.Join(",", context.Message.Ids);
                    }

                    if (context.Message.Action == EntityState.Deleted)
                    {
                        emptyField.OldValue = string.Join(",", context.Message.Ids);
                    }
                }

                var dataChanges = new List<DataChange>();

                if (fields.Any())
                {
                    dataChanges.AddRange(fields.Where(f => !string.IsNullOrWhiteSpace(f.Name)).Select(f =>
                        new DataChange
                        {
                            ColumnName = f.Name,
                            OldValue = f.OldValue,
                            NewValue = f.NewValue,
                        }).ToList());
                }

                await _service.HandleAsync(new AuditLogService.InsertAuditLogInMemory
                {
                    EntityId = ids.First().Id,
                    EntityIdAdditional = ids.Count > 1 ? ids.Skip(1).First().Id : null,
                    Action = GetAction(context.Message.Action),
                    Updated = context.Message.Updated,
                    UserName = context.Message.UserName,
                    TableName = context.Message.Name,
                    Changes = dataChanges,
                }, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }

    private static IEnumerable<(int Id, bool Converted)> GetIntValue(object pk)
    {
        switch (pk)
        {
            case int intValue:
                yield return (intValue, false);
                break;
            case long longValue:
                yield return ((int) longValue, longValue > int.MaxValue);
                break;
            case Guid guidValue:
                yield return (BitConverter.ToInt32(guidValue.ToByteArray(), 0), true);
                yield return (BitConverter.ToInt32(guidValue.ToByteArray(), 4), true);
                break;
            case string stringValue:
                if (Guid.TryParse(stringValue, out var guidValue2))
                {
                    var bytes = guidValue2.ToByteArray();
                    yield return (BitConverter.ToInt32(bytes, 0), true);
                    yield return (BitConverter.ToInt32(bytes, 4), true);
                }
                else if (int.TryParse(stringValue, out var intValue2))
                {
                    yield return (intValue2, false);
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(stringValue);
                    var padded = bytes.Concat(Enumerable.Repeat<byte>(0, 3)).ToArray();
                    if (bytes.Length > 0)
                    {
                        yield return (BitConverter.ToInt32(padded, 0), true);
                    }

                    if (bytes.Length > 4)
                    {
                        yield return (BitConverter.ToInt32(padded, 4), true);
                    }
                }

                break;
        }
    }

    private static string GetAction(EntityState action)
    {
        return action switch
        {
            EntityState.Added => "Insert",
            EntityState.Modified => "Update",
            EntityState.Deleted => "Delete",
            _ => action.ToString(),
        };
    }
}
