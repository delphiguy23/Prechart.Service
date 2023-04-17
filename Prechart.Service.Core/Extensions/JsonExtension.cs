using Newtonsoft.Json;

namespace Prechart.Service.Core.Extensions;

public static class JsonExtension
{
    public static string ToJsonString<T>(this T data)
    {
        if (data is null)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(data);
    }

    public static T FromJsonStringToObject<T>(this string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(data);
    }
}
