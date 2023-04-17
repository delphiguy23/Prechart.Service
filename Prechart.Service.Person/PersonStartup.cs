using System;
using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Service;
using Prechart.Service.Person.Consumer;
using Prechart.Service.Person.Models;
using Prechart.Service.Person.Repositories.Daywage;
using Prechart.Service.Person.Repositories.Person;
using Prechart.Service.Person.Services.Daywage;
using Prechart.Service.Person.Services.Person;

namespace Prechart.Service.Person;

public class PersonStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<PersonRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<PersonService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<DaywageService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<DaywageRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                var database = client.GetDatabase(config.MongoDb.Database);
                var collection = database.GetCollection<PersonModel>("Persons");
                return collection;
            });
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.UseMessageRetry(r => r.Incremental(
            10,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5)));

        ep.ConfigureConsumer<XmlToPersonConsumer>(provider);
        ep.ConfigureConsumer<PersonUserCredentialsConsumer>(provider);
        ep.ConfigureConsumer<UpsertPersonUserCredenitalsConsumer>(provider);
        ep.ConfigureConsumer<ResetPersonUserCredenitalsConsumer>(provider);
        ep.ConfigureConsumer<UpdatePersonPhotoConsumer>(provider);
        ep.ConfigureConsumer<DownloadPersonPhotoConsumer>(provider);
        ep.ConfigureConsumer<CreateTestPortalUser>(provider);
    }
}
