namespace Prechart.Service.Core.Events;

public interface IGetAuditTrail
{
    string TableName { get; }
    int TableId { get; }
}