using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prechart.Service.Belastingen.Consumers;
using Prechart.Service.Belastingen.Database.Context;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Repositories.Berekeningen;
using Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Services.Berekeningen;
using Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Belastingen;

public class Belastingen : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<BelastingTabellenWitGroenRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<PremiePercentagesSocialeVerzekeringenRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BelastingTabellenWitGroenService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<PremiePercentagesSocialeVerzekeringenService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BerekeningenService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BerekeningenRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();

        builder.Register(c =>
        {
            var config = c.Resolve<IOptions<GeneralConfiguration>>().Value;
            var opt = new DbContextOptionsBuilder<BelastingenDbContext>();
            opt.UseSqlServer(config.ConnectionString);
            return new BelastingenDbContext(opt.Options, c.Resolve<ISaveDatabaseHelper>());
        }).AsImplementedInterfaces().InstancePerLifetimeScope();
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.UseMessageRetry(r => r.Immediate(5));
        ep.ConfigureConsumer<GetWoonlandbeginselConsumer>(provider);
        ep.ConfigureConsumer<InsertToTaxTableConsumer>(provider);
        ep.ConfigureConsumer<GetInhoudingConsumer>(provider);
        ep.ConfigureConsumer<PremieBedragConsumer>(provider);
        ep.ConfigureConsumer<PendingCsvBatchProcessConsumer>(provider);
    }
}
