using Autofac;
using MassTransit;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Helper;

namespace Prechart.Service.Globals;

public class GlobalsStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<XsdHelper>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BatchHelper>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<MongoDbHelper>().AsImplementedInterfaces().InstancePerLifetimeScope();
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
    }
}
