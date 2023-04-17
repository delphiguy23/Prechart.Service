using System.Collections.Generic;
using System.Linq;

namespace Prechart.Service.Core.FluentResults.Extension;

public static class MessageExtension
{
    public static bool Containing(this List<string> source, string toCheck)
    {
        return source?.Any(m => m.Contains(toCheck)) ?? false;
    }
}