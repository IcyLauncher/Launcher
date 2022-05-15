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

            })
            .Build();

        Provider = host.Services;
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await host.StartAsync();

        var shell = Provider.GetRequiredService<ShellView>();
        shell.Activate();
    }
}