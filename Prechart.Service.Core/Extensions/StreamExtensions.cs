using System.IO;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Extensions;

public static class StreamExtensions
{
    public static async Task<byte[]> ToByteArray(this Stream stream)
    {
        var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}
