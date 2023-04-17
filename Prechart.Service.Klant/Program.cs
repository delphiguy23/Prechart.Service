using Prechart.Service.Core;

namespace Prechart.Service.Klant;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Bootstrap.Start(args);
    }
}

