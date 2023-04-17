namespace Prechart.Service.Globals.Helper;

public interface IMongoDbHelper
{
    void TryClassMapRegistration<T>(Type param);
}