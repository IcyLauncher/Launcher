using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IcyLauncher.WinUI;

public partial class App : Application
{
    readonly IHost host;

    public static IServiceProvider Provider { get; private set; } = default!;
    public static InMemorySink Sink { get; private set; } = new();


    public App()
    {
        host = Host.CreateDefaultBuilder()
            .UseSerilog((context, configuration) =>
            {
                configuration.WriteTo.Debug();
                configuration.WriteTo.Sink(Sink);
                configuration.WriteTo.File("Logs\\Log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10);
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("Configuration.json", true);
                builder.AddJsonFile("Assets\\Banners\\SolidColors.json", true);
            })
            .ConfigureServices((context, services) =>
            {
                // Configuration
                services.Configure<Configuration>(context.Configuration);
                services.Configure<SolidColorCollection>(context.Configuration);

                Configuration configuration = context.Configuration.Get<Configuration>();

                // Managers
                services.AddScoped<Services.ConfigurationManager>();
                services.AddScoped<ThemeManager>();
                // App/UI Handlers
                services.AddScoped<AppStartupHandler>();
                services.AddScoped<WindowHandler>();
                services.AddScoped<UIElementReciever>();
                services.AddScoped<MicaBackdropHandler>();
                services.AddScoped<AcrylicBackdropHandler>();
                services.AddScoped<VibrancyBackdropHandler>();
                services.AddScoped<BackdropHandler>();
                // Utilities
                services.AddScoped<IConverter, JsonConverter>();
                services.AddScoped<ImagingUtility>();
                // Local
                services.AddScoped<IFileSystem, FileSystem>();
                services.AddScoped<Updater>();
                // Navigation
                services.AddScoped<INavigation, Navigation>();
                services.AddScoped<IMessage, Message>();
                // ViewModels
                services.AddSingleton<NoPageViewModel>();
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<ProfilesViewModel>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<BannerSettingsViewModel>();
                services.AddSingleton<ColorSettingsViewModel>();
                services.AddSingleton<DeveloperSettingsViewModel>();

                // Window
                services.AddSingleton<Window>(provider => new()
                {
                    Content = UIElementProvider.MainGrid(
                        new GridLength[] { new(), new(1, GridUnitType.Star) },
                        UIElementProvider.TitleBar(configuration.Apperance.Colors.Accent.Light, configuration.Apperance.Colors.Accent.Dark),
                        UIElementProvider.NavigationView()),
                    Title = "IcyLauncher"
                });
            }).Build();

        Provider = host.Services;
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await host.StartAsync().ConfigureAwait(false);

        Provider.GetRequiredService<AppStartupHandler>();
    }
}