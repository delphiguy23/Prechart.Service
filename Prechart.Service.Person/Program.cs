using System.Threading.Tasks;
using Prechart.Service.Core;

namespace Prechart.Service.Employee
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Bootstrap.Start(args);
        }
    }
}
