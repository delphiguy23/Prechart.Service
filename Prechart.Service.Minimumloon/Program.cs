using Prechart.Service.Core;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Bootstrap.Start(args);
        }
    }
}