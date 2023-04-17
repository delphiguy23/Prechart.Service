using System;
using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Service;
using Prechart.Service.Loonheffings.Consumer;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;
using Prechart.Service.Loonheffings.Service;

namespace Prechart.Service.Loonheffings;

public class DocumentsStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<LoonheffingsService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<LoonheffingsRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();

        builder.Register<IMongoClient>(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                return client;
            });

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                var database = client.GetDatabase(config.MongoDb.Database);
                var collection = database.GetCollection<XmlLoonaangifteUpload>("Loonaangiftes");
                return collection;
            });
    }

    public override void RegisterConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeneralConfiguration>(configuration.GetSection("General"));
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.UseMessageRetry(r => r.Incremental(
            10,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5)));

        ep.ConfigureConsumer<LoonheffingsConsumer>(provider);
        ep.ConfigureConsumer<LoonheffingsProcessedResultConsumer>(provider);
        ep.ConfigureConsumer<PendingXmlBatchProcessConsumer>(provider);
    }
}
