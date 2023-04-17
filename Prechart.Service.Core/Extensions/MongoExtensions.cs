using MongoDB.Bson;

namespace Prechart.Service.Core.Extensions;

public static class MongoExtensions
{
    public static ObjectId ToObjectId(this string id)
    {
        return ObjectId.TryParse(id, out var objectId) ? objectId : ObjectId.Empty;
    }

    public static ObjectId ToObjectIdOrCreateIfEmpty(this string id)
    {
        return ObjectId.TryParse(id, out var objectId) ? objectId : ObjectId.GenerateNewId();
    }
}
