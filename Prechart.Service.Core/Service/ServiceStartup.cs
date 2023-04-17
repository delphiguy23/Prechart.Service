using Autofac;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Prechart.Service.Core.Service
{
    public abstract class ServiceStartup : Autofac.Module, IServiceStartup
    {
        public virtual void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
        {
        }

        public virtual void RegisterConfiguration(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
        }

        public virtual void ConfigureAutoFac(ContainerBuilder builder)
        {
        }

        public virtual void ConfigureAuthorization(AuthorizationOptions options)
        {
        }

        protected sealed override void Load(ContainerBuilder builder) => ConfigureAutoFac(builder);
    }
}
