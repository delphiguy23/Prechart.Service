using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.Batch.Consumers;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Batch.Services;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Batch;

public class BatchStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<BatchService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BatchRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                var database = client.GetDatabase(config.MongoDb.Database);
                var collection = database.GetCollection<BatchProcess>("BatchProcess");
                return collection;
            });
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.UseMessageRetry(r => r.Immediate(5));
        ep.ConfigureConsumer<CreateBatchRecordConsumer>(provider);
        ep.ConfigureConsumer<PendingBatchProcessConsumer>(provider);

        ep.ConfigureConsumer<UpdateBatchErrorsConsumer>(provider);
        ep.ConfigureConsumer<UpdateBatchProgressConsumer>(provider);
        ep.ConfigureConsumer<UpdateBatchStatusConsumer>(provider);
    }
}
