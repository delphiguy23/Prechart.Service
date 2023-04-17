using MongoDB.Bson;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Globals.Interfaces.Batch;

public interface IBatchProcessModel
{
    ObjectId Id { get; set; }
    string BatchName { get; set; }
    int TotalTask { get; set; }
    int CompletedTask { get; set; }
    BatchProcessStatus Status { get; set; }
    DateTime PublishedOn { get; set; }
    DateTime? ScheduledOn { get; set; }
    DateTime? StartedOn { get; set; }
    DateTime? FinalizedOn { get; set; }
    string Payload { get; set; }
    List<string> BatchErrors { get; set; }
}