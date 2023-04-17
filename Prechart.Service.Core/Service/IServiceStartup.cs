using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Prechart.Service.Core.Service
{
    public interface IServiceStartup
    {
        void RegisterConfiguration(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration);
        void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider);
        void ConfigureAuthorization(AuthorizationOptions options);
    }
}
