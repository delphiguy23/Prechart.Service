using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using MassTransit.MessageData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Prechart.Service.Core.Authorization;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.DiskStorage;
using Prechart.Service.Core.Events;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Core.Scheduling;
using Prechart.Service.Core.Service;
using Prechart.Service.Core.Trackers;

namespace Prechart.Service.Core.Extensions
{
    public static class AutofacExtensions
    {
        public static IContainer AddAutoFacSetup(
            this IServiceCollection services,
            Assembly[] assemblies,
            IEnumerable<IServiceStartup> serviceStartups,
            GeneralConfiguration general,
            IWebHostEnvironment environment)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAssemblyModules(assemblies);

            RegisterMassTransit(assemblies, serviceStartups, general, builder, environment);

            foreach (var recurringTask in assemblies
                         .SelectMany(s => s.GetTypes())
                         .Where(p => typeof(IRecurringTask).IsAssignableFrom(p) && p != typeof(IRecurringTask) &&
                                     !p.IsAbstract && !p.IsInterface))
            {
                builder.RegisterType(recurringTask).AsImplementedInterfaces().InstancePerLifetimeScope();
            }

            builder.RegisterType<TokenHelper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SaveDatabaseHelper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AuditLogTracker>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ServiceCall>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DiskStorageProvider>().AsImplementedInterfaces().SingleInstance();

            builder.Register(c => new HttpClient
            {
                Timeout = new TimeSpan(0, 59, 0),
            }).AsSelf().InstancePerLifetimeScope();

            var container = builder.Build();
            var bc = container.Resolve<IBusControl>();
            bc.Start();
            return container;
        }

        private static void RegisterMassTransit(
            Assembly[] assemblies,
            IEnumerable<IServiceStartup> serviceStartups,
            GeneralConfiguration general,
            ContainerBuilder builder,
            IWebHostEnvironment environment)
        {
            var consumers = assemblies
                .SelectMany(s => s.GetTypes().Where(p =>
                    typeof(IConsumer).IsAssignableFrom(p) && p != typeof(IConsumer) && !p.IsAbstract));

            if (environment.IsDevelopment())
            {
                foreach (var pair in consumers.Select(t => t.GetEventType())
                             .Where(t => !t.IsAssignableTo<IWrappedMessage>()).Distinct().Select(c =>
                                 new EventTypePair {EventType = c, EventMockType = c.GetEventTypeMock()}))
                {
                    builder.RegisterInstance(pair).AsSelf().SingleInstance();
                }
            }

            builder.AddMassTransit(x =>
            {
                foreach (var consumerType in consumers)
                {
                    x.AddConsumer(consumerType);
                }

                if (!string.IsNullOrWhiteSpace(general.RabbitMq?.HostName))
                {
                    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.UseMessageData(new InMemoryMessageDataRepository());

                        cfg.Host(
                            new Uri(
                                $"rabbitmq://{general.RabbitMq?.HostName}:{general.RabbitMq?.Port}/{general.RabbitMq?.VHost}"),
                            h =>
                            {
                                h.Username(general.RabbitMq?.User);
                                h.Password(general.RabbitMq?.Password);
                            });

                        cfg.AddReceiveEndpoints(provider, serviceStartups, general);
                    }));
                }
                else
                {
                    x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                    {
                        cfg.UseMessageData(new InMemoryMessageDataRepository());

                        var queuenamePrefix =
                            $"{general.Environment.ToLowerInvariant()}_{general.Tenant.ToLowerInvariant()}_";
                        cfg.AddReceiveEndpoints(provider, serviceStartups, general, queuenamePrefix);
                    }));
                }
            });
        }

        private static void AddReceiveEndpoints<TEndpointConfigurator>(
            this IReceiveConfigurator<TEndpointConfigurator> cfg, IBusRegistrationContext provider,
            IEnumerable<IServiceStartup> serviceStartups, GeneralConfiguration general, string queuenamePrefix = "")
            where TEndpointConfigurator : IReceiveEndpointConfigurator
        {
            var queueNames = new List<string>();

            foreach (var consumerConfigurator in serviceStartups.Distinct().ToList())
            {
                var msname = consumerConfigurator.GetType().Namespace.ToLowerInvariant()
                    .Replace(".", string.Empty, StringComparison.InvariantCultureIgnoreCase);

                if (msname.StartsWith("PrechartService", StringComparison.InvariantCultureIgnoreCase))
                {
                    var queue = $"{queuenamePrefix}{msname.Substring(15)}";

                    if (!queueNames.Contains(queue))
                    {
                        queueNames.Add($"{queuenamePrefix}{msname.Substring(15)}");

                        cfg.ReceiveEndpoint(queue, ep => { consumerConfigurator.ConfigureMessageBus(ep, provider); });
                    }
                }
            }
        }

        private static IMessageDataRepository FileMessageRepository(string path)
        {
            var dataDirectory = new DirectoryInfo(path);

            return new FileSystemMessageDataRepository(dataDirectory);
        }
    }
}