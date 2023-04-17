using Prechart.Service.Globals.Models.Documents;

namespace Prechart.Service.Globals.Helper;

public static class DocumentsHelper
{
    public static MediaType StringToMediaType(this string mediaType)
    {
        return (MediaType)Enum.Parse(typeof(MediaType), mediaType);
    }

    public static MediaSubtype StringToMediaSubtype(this string mediaType)
    {
        return (MediaSubtype)Enum.Parse(typeof(MediaSubtype), mediaType);
    }
}
