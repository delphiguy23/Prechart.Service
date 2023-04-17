using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MassTransit;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prechart.Service.Core.AppInsights;
using Prechart.Service.Core.Authorization;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.Helper;
using Prechart.Service.Core.Scheduling;
using Prechart.Service.Core.Service;
using Serilog;

namespace Prechart.Service.Core;

public class Startup
{
    private readonly string allowSpecificOrigins = "_AllowSpecificOrigins";

    public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
    {
        Configuration = configuration;
        Environment = environment;
        LoggerFactory = loggerFactory;
        Assemblies = DependencyContext.Default.RuntimeLibraries
            .Where(d => d.Name.StartsWith("Prechart.", StringComparison.InvariantCultureIgnoreCase))
            .Select(n => Assembly.Load(new AssemblyName(n.Name)))
            .ToArray();

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Converters = new List<JsonConverter>(new[]
            {
                new StringEnumConverter(),
            }),
        };
    }

    private IConfiguration Configuration { get; }
    public ILoggerFactory LoggerFactory { get; }
    public IWebHostEnvironment Environment { get; }
    private Assembly[] Assemblies { get; }
    private IContainer ApplicationContainer { get; set; }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        var serviceStartups = Assemblies
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IServiceStartup).IsAssignableFrom(p) && p != typeof(IServiceStartup) && !p.IsAbstract)
            .Select(type => (IServiceStartup) Activator.CreateInstance(type)).ToList();

        services.AddCors(options => { options.AddPolicy(allowSpecificOrigins, builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });

        services.AddJwtSetup(Configuration);

        var config = Configuration.GetSection("General");
        var generalConfig = new GeneralConfiguration();
        config.Bind(generalConfig);

        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
            .AddResponseCompression()
            .AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
                {DeveloperMode = Debugger.IsAttached, InstrumentationKey = generalConfig?.ApplicationInsightsInstrumentationKey ?? string.Empty, EnableDebugLogger = false, EnableAdaptiveSampling = false})
            .AddMemoryCache()
            .AddSwaggerSetup()
            .AddHealthCheckSetup(generalConfig)
            .AddMvcCore(options => { options.AllowEmptyInputInBodyModelBinding = true; })
            .AddDataAnnotations()
            .ConfigureApplicationPartManager(apm =>
            {
                foreach (var assembly in Assemblies)
                {
                    apm.ApplicationParts.Add(new AssemblyPart(assembly));
                }
            })
            .AddControllersAsServices()
            .AddApiExplorer()
            .AddNewtonsoftJson(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); })
            .AddAuthorization(a =>
            {
                foreach (var instance in serviceStartups)
                {
                    instance.ConfigureAuthorization(a);
                }

                a.AddPolicy(
                    "PrechartPlaceholder",
                    policyBuilder => policyBuilder
                        .RequireAssertion(
                            context =>
                                (context.User.Claims.HasAccessRole("placeholder") && context.User.Claims.IsService())
                                || context.User.Claims.HasRole("Administrator")));

                a.AddPolicy(
                    "ServiceClient",
                    policyBuilder => policyBuilder
                        .Requirements.Add(new ServiceAccessRequirement()));
            });
        services.AddHttpContextAccessor()
            .AddOptions()
            .AddAutoMapper(Assemblies);

        services.Configure<GeneralConfiguration>(config);
        services.Configure<TestConfiguration>(Configuration.GetSection("Test"));
        foreach (var instance in serviceStartups)
        {
            instance.RegisterConfiguration(services, Configuration);
        }

        var queueName = string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.FriendlyName) ? "default_schema" : AppDomain.CurrentDomain.FriendlyName.ToLower().Replace('.', '_');

        services.AddHangfire((context, configuration) => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(new MongoClient(context.GetService<IOptions<GeneralConfiguration>>().Value.MongoDb.ConnectionString),
                $"{context.GetService<IOptions<GeneralConfiguration>>().Value.MongoDb.Database}-HangFire",
                new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy(),
                    },
                    Prefix = queueName,
                    CheckConnection = true,
                }));

        services.AddHangfireServer();
        services.AddSingleton<IAuthorizationHandler, ServiceAccessAuthorizationHandler>();

        services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();
            logBuilder.AddConsole();
            var log = SerilogHelper.CreateSerilogLogger(generalConfig.MongoDb.LogConnectionString).CreateLogger();
            logBuilder.AddSerilog(log, true);
        });

        ApplicationContainer = services.AddAutoFacSetup(Assemblies, serviceStartups, generalConfig, Environment);
        GlobalConfiguration.Configuration.UseAutofacActivator(ApplicationContainer);

        return new AutofacServiceProvider(ApplicationContainer);
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ILoggerFactory loggerFactory,
        TelemetryConfiguration configuration,
        IHostApplicationLifetime lifeTime)
    {
        var builder = configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
        builder.UseAdaptiveSampling(5, "Trace;Exception;Event");
        builder.Use(next => new IgnoreNoiseProcessor(next));

        builder.Build();

        (env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseHsts().UseHttpsRedirection())
            .UseResponseCompression()
            .Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(HeaderNames.CacheControl))
                    {
                        context.Response.Headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
                    }

                    return Task.CompletedTask;
                });
                await next();
            })
            .UseRouting()
            .UseCors(allowSpecificOrigins)
            .UseHealthCheckSetup()
            .UseSwaggerSetup(env.IsDevelopment())
            .UseMiddleware(typeof(AccessTokenQueryStringMiddleware))
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers().RequireAuthorization())
            .UseHangfireDashboard("/service/hangfire", new DashboardOptions
            {
                Authorization = new[] {new BearerAuthorizationFilter(ApplicationContainer.Resolve<IOptions<GeneralConfiguration>>(), ApplicationContainer.Resolve<ITokenHelper>(), ApplicationContainer.Resolve<IWebHostEnvironment>())},
            })
            ;
        var log = loggerFactory.CreateLogger<Startup>();
        AfterStartInitialization(log, env);

        AppDomain.CurrentDomain.UnhandledException += (s, ev) => { log.LogError(ev.ExceptionObject as Exception, $"An unhandled exception occurred: {(Exception) ev.ExceptionObject}"); };

        lifeTime.ApplicationStopping.Register(() => ApplicationContainer.Resolve<IBusControl>()?.Stop());
    }

    private void AfterStartInitialization(ILogger<Startup> logger, IWebHostEnvironment env)
    {
        using (var scope = ApplicationContainer.BeginLifetimeScope())
        {
            var disableRecurringTasks = env.IsDevelopment() && !scope.Resolve<IOptions<TestConfiguration>>().Value.RecurringTasks;

            var initializableObjects = scope.Resolve<IEnumerable<IInitializable>>();
            foreach (var initializableObject in initializableObjects)
            {
                if (disableRecurringTasks && initializableObject is IRecurringTask recurringTask)
                {
                    RecurringJob.RemoveIfExists(recurringTask.Name);
                    continue;
                }

                try
                {
                    logger.LogDebug($"Init {initializableObject.GetType().Name}");
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                    Task.Run(() => initializableObject.Init()).Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error during init {initializableObject.GetType().Name}");
                    throw;
                }
            }
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var contentPath = Path.GetDirectoryName(assembly.Location ?? string.Empty);

                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    config.AddJsonFile(Path.Combine(contentPath, "appsettings.Local.json"), true, false);
                    config.AddJsonFile(Path.Combine(contentPath, "appsettings.Development.json"), true, false);
                }
                else
                {
                    var configPath = Path.Combine(contentPath, "config");
                    config.AddJsonFile(Path.Combine(configPath, "appsettings.json"), true, false);
                }
            })
            .ConfigureKestrel(o => o.AddServerHeader = false)
            .UseStartup<Startup>();
    }
}
