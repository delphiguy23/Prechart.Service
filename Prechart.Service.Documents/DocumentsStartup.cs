using Autofac;
using MassTransit;
using Prechart.Service.Core.Service;
using Prechart.Service.Documents.Upload.Csv.Services;

namespace Prechart.Service.Documents.Upload.Csv;

public class DocumentsStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<DocumentsService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();
    }
}
