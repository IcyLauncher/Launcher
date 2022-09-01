using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IcyLauncher.WinUI;

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
                builder.AddJsonFile("Assets\\Banners\\SolidColors.json", true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<Configuration>(context.Configuration);
                services.Configure<SolidColorCollection>(context.Configuration);

                var configuration = context.Configuration.Get<Configuration>();

                services.AddScoped<Services.ConfigurationManager>();
                services.AddScoped<ThemeManager>();
                services.AddScoped<WindowHandler>();
                services.AddScoped<UIElementReciever>();
                services.AddScoped<ImagingUtility>();
                services.AddScoped<AppStartupHandler>();
                services.AddScoped<Updater>();
                services.AddScoped<IConverter, JsonConverter>();
                services.AddScoped<IFileSystem, FileSystem>();
                services.AddScoped<IMessage, Message>();
                services.AddScoped<INavigation, Navigation>();
                services.AddScoped<MicaBackdropHandler>();
                services.AddScoped<AcrylicBackdropHandler>();

                services.AddSingleton<NoPageViewModel>();
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<ProfilesViewModel>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<BannerSettingsViewModel>();
                services.AddSingleton<ColorSettingsViewModel>();
                services.AddSingleton<DeveloperSettingsViewModel>();

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
        await host.StartAsync();

        Provider.GetRequiredService<AppStartupHandler>();
    }
}