using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.SharedTypes;

namespace Prechart.Service.Core.AppInsights
{
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        private readonly GeneralConfiguration _generalConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomTelemetryInitializer(IOptions<GeneralConfiguration> generalConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _generalConfiguration = generalConfiguration.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
        {
            telemetry.Timestamp = DateTime.Now.Add(InternetTime.GetTimeOffset());

            var propTelemetry = (ISupportProperties)telemetry;
            if (!propTelemetry.Properties.ContainsKey("ServerTime"))
            {
                propTelemetry.Properties.Add("ServerTime", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture));
            }

            if (!propTelemetry.Properties.ContainsKey("Environment"))
            {
                propTelemetry.Properties.Add("Environment", _generalConfiguration.Environment);
            }

            if (!propTelemetry.Properties.ContainsKey("Tenant"))
            {
                propTelemetry.Properties.Add("Tenant", _generalConfiguration.Tenant);
            }

            if (!propTelemetry.Properties.ContainsKey("User"))
            {
                propTelemetry.Properties.Add("User", _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty);
            }

            if (!propTelemetry.Properties.ContainsKey("ServiceName"))
            {
                propTelemetry.Properties.Add("ServiceName", _generalConfiguration.ServiceName);
            }
        }
    }
}
