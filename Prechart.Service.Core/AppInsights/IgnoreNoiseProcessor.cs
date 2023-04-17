using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Prechart.Service.Core.AppInsights
{
    public class IgnoreNoiseProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;

        public IgnoreNoiseProcessor(ITelemetryProcessor next) => _next = next;

        public void Process(ITelemetry item)
        {
            if (IsNoise(item))
            {
                return;
            }

            _next.Process(item);
        }

        private static bool IsNoise(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency?.Type == "SQL" && dependency?.Success == true)
            {
                return true;
            }

            return false;
        }
    }
}
