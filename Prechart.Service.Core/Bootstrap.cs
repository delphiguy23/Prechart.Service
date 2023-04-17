using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Prechart.Service.Core;

public class Bootstrap : BackgroundService
{
    private readonly IHostApplicationLifetime hostApplicationLifetime;
    private readonly ILogger<Bootstrap> logger;

    public Bootstrap(ILogger<Bootstrap> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.logger = logger;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    public static IEnumerable<string> Args { get; set; }

    public static async Task Start(string[] args)
    {
        var exePath = Directory.GetCurrentDirectory();

        if (!Debugger.IsAttached)
        {
            var pathToExe = Environment.ProcessPath;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            Directory.SetCurrentDirectory(pathToContentRoot);
        }

        Directory.SetCurrentDirectory(Directory.GetParent(exePath).FullName);

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) => { services.AddHostedService<Bootstrap>(); })
            .UseWindowsService()
            .Build();

        Args = args;

        await host.RunAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Creating service");
            var host = Startup.CreateWebHostBuilder(
                Args.ToArray()).Build();

            logger.LogInformation("Starting service");
            await host.RunAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Service");
            throw;
        }

        if (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Stopping service");
            hostApplicationLifetime.StopApplication();
        }
    }
}
