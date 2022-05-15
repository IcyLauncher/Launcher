using IcyLauncher.Models;
using IcyLauncher.Services;
using IcyLauncher.Services.Interfaces;
using IcyLauncher.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml.Controls;
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
                NavigationView navigationView = INavigation.CreateNew();

                services.AddScoped<INavigation>(provider => new Navigation(navigationView, (Frame)navigationView.Content));
                services.AddScoped<IConverter, JsonConverter>();

                services.Configure<Configuration>(context.Configuration);

                services.AddSingleton<ShellViewModel>();

                services.AddSingleton<ShellView>(provider => new() { Content = navigationView });
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