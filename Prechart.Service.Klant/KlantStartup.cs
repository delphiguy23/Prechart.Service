using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Consumers;
using Prechart.Service.Klant.Repository;
using Prechart.Service.Klant.Service;

namespace Prechart.Service.Klant;

public class KlantStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<KlantRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<KlantService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                var database = client.GetDatabase(config.MongoDb.Database);
                var collection = database.GetCollection<KlantModel>("Klants");
                return collection;
            });
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.UseMessageRetry(r => r.Incremental(
            10,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5)));

        ep.ConfigureConsumer<UpsertKlantsConsumer>(provider);
        ep.ConfigureConsumer<GetAllKlantsConsumer>(provider);
        ep.ConfigureConsumer<GetKlantConsumer>(provider);
    }
}
