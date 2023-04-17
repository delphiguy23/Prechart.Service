using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Prechart.Service.AuditLog.Models;

namespace Prechart.Service.AuditLog.Worker;

public class MongoChangeStreamWorker : IMongoChangeStreamWorker
{
    private readonly ILogger<MongoChangeStreamWorker> _logger;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<MongoLogs> _mongoLogs;

    public MongoChangeStreamWorker(ILogger<MongoChangeStreamWorker> logger,
        IMongoDatabase mongoDatabase,
        IMongoCollection<MongoLogs> mongoLogs)
    {
        _logger = logger;
        _mongoDatabase = mongoDatabase;
        _mongoLogs = mongoLogs;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
        var options = new ChangeStreamOptions {FullDocument = ChangeStreamFullDocumentOption.UpdateLookup};
        var operations = new[]
        {
            ChangeStreamOperationType.Insert,
            ChangeStreamOperationType.Update,
            ChangeStreamOperationType.Replace,
            ChangeStreamOperationType.Delete,
            ChangeStreamOperationType.Invalidate,
        };

        try
        {
            using (var changeStreamCursor = await _mongoDatabase.WatchAsync(options, stoppingToken))
            {
                _logger.LogInformation("Start Watching database {String}", _mongoDatabase.DatabaseNamespace.DatabaseName);

                while (await changeStreamCursor.MoveNextAsync())
                {
                    foreach (var csd in changeStreamCursor.Current)
                    {
                        if (operations.Contains(csd.OperationType))
                        {
                            await _mongoLogs.InsertOneAsync(new MongoLogs
                            {
                                ChangeDateTime = DateTime.Now,
                                Database = csd.DatabaseNamespace.DatabaseName,
                                Collection = csd.CollectionNamespace.CollectionName,
                                CollectionDocumentId = csd.DocumentKey,
                                OperationType = Enum.GetName(typeof(ChangeStreamOperationType), csd.OperationType),
                                UpdateDescription = csd.UpdateDescription.ToBsonDocument(),
                            });
                        }
                    }
                }

                _logger.LogInformation("End Watching database {String}", _mongoDatabase.DatabaseNamespace.DatabaseName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
