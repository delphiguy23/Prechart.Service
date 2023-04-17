using System.Threading.Tasks;
using Prechart.Service.Core;

namespace Prechart.Service.Users
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Bootstrap.Start(args);
        }
    }
}