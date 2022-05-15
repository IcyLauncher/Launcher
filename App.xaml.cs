using IcyLauncher.Models;
using IcyLauncher.Services;
using IcyLauncher.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IcyLauncher;

public partial class App : Application
{
    readonly IHost host;
    public static IServiceProvider Provider { get; private set; } = default!;

    public App()
    {
        host = Host.CreateDefaultBuilder()
            .UseSerilog((context, configuration) =>
            {
                configuration.WriteTo.Debug();
                configuration.WriteTo.File("Logs\\Log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10);
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("Configuration.json", true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IConverter, JsonConverter>();

                services.Configure<Configuration>(context.Configuration);
            })
            .Build();

        Provider = host.Services;

        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
        {
            var logger = Provider.GetRequiredService<ILogger<App>>();
            logger.Log("Global FirstChanceException", args.Exception);
        };
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await host.StartAsync();

        var shell = Provider.GetRequiredService<ShellView>();
        shell.Activate();
    }
}