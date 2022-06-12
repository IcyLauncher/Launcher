using IcyLauncher.ViewModels;
using IcyLauncher.Views;
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
                services.Configure<Configuration>(context.Configuration);
                var configuration = context.Configuration.Get<Configuration>();

                var titleBar = UIElementProvider.TitleBar(configuration.Apperance.Colors.Accent.Light, configuration.Apperance.Colors.Accent.Dark, out Button backButton);
                var navigationView = UIElementProvider.NavigationView(out Frame contentFrame);
                var mainGrid = UIElementProvider.MainGrid(new GridLength[] { new(), new(1, GridUnitType.Star) }, 
                    titleBar, navigationView);

                services.AddScoped<Core.Services.ConfigurationManager>();
                services.AddScoped<ThemeManager>();
                services.AddScoped<WindowHandler>();
                services.AddScoped<IConverter, JsonConverter>();
                services.AddScoped<INavigation>(provider => new Navigation(provider.GetRequiredService<ILogger<Navigation>>(), navigationView, contentFrame, backButton));
                services.AddScoped<ControlReciever>();
                services.AddScoped<AppStartupHandler>();

                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<ProfilesViewModel>();

                services.AddSingleton<Window>(provider => new ShellView() { Content = mainGrid, Title = "IcyLauncher" });
            })
            .Build();

        Provider = host.Services;
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await host.StartAsync();
        Provider.GetRequiredService<AppStartupHandler>();
    }
}