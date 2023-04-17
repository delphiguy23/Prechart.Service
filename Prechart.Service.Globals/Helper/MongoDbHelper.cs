using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;

namespace Prechart.Service.Globals.Helper;

public class MongoDbHelper : IMongoDbHelper
{
    private readonly ILogger<MongoDbHelper> _logger;

    public MongoDbHelper(ILogger<MongoDbHelper> logger)
    {
        _logger = logger;
    }

    public void TryClassMapRegistration<T>(Type param)
    {
        if (!BsonClassMap.IsClassMapRegistered(param))
        {
            try
            {
                BsonClassMap.RegisterClassMap<T>();
            }
            catch (Exception)
            {
                _logger.LogWarning($"{nameof(T)} is already registered.");
            }
        }
    }
}
