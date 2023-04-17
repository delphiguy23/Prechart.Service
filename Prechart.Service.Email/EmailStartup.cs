using Autofac;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Service;
using Prechart.Service.Email.Consumers;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Prechart.Service.Email.Service;

namespace Prechart.Service.Email;

public class EmailStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<EmailService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<EmailRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();


        builder.Register(
        m =>
        {
            var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
            var client = new MongoClient(config.MongoDb.ConnectionString);
            var database = client.GetDatabase(config.MongoDb.MailArchiveDatabase);
            var collection = database.GetCollection<EmailEventModel>("EmailEvent");
            return collection;
        });

        builder.Register(
         m =>
         {
             var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
             var client = new MongoClient(config.MongoDb.ConnectionString);
             var database = client.GetDatabase(config.MongoDb.Database);
             var collectionEmailEventRecipient = database.GetCollection<EmailEventRecipientModel>("EmailEventRecipient");
             return collectionEmailEventRecipient;
         });
        builder.RegisterGenericRequestClient();
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.ConfigureConsumer<EmailEventConsumer>(provider);
    }
}
