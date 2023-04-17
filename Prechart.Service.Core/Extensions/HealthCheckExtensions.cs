using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using Prechart.Service.Core.Configuration;

namespace Prechart.Service.Core.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthCheckSetup(this IServiceCollection services, GeneralConfiguration generalConfiguration)
        {
            var builder = services.AddHealthChecks()
                .AddSqlServer(generalConfiguration.ConnectionString)
                .AddHangfire(setup =>
                {
                    setup.MinimumAvailableServers = 1;
                    setup.MaximumJobsFailed = 20;
                });
            
            if (!string.IsNullOrEmpty(generalConfiguration.RabbitMq?.HostName))
            {
                builder.AddRabbitMQ($"amqp://{generalConfiguration.RabbitMq?.User}:{generalConfiguration.RabbitMq?.Password}@{generalConfiguration.RabbitMq?.HostName}:{generalConfiguration.RabbitMq?.Port}/{generalConfiguration.RabbitMq?.VHost}", name: "rabbitmqbus-check");
            }
            
            return services;
        }

        public static IApplicationBuilder UseHealthCheckSetup(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/service/general/healthcheck", new HealthCheckOptions()
            {
                ResponseWriter = WriteResponse,
            });

            return app;
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = result.Status == HealthStatus.Healthy ? 200 : 500;

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
