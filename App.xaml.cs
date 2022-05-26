using IcyLauncher.Helpers;
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
    public static bool CanGoBack 
    {
        get
        {
            return backButton.Visibility != Visibility.Collapsed;
        }
        set
        {
            if (value)
            {
                backButton.Visibility = Visibility.Visible;
                UIElementProvider.Animate(backButton, "Opacity", 0, 1, 200).Begin();
                UIElementProvider.Animate(backButton, "Width", 0, 32, 110).Begin();
            } 
            else
            {
                UIElementProvider.Animate(backButton, "Opacity", 1, 0, 200).Begin();
                var board = UIElementProvider.Animate(backButton, "Width", 32, 0, 110);
                board.Completed += (s, e) => backButton.Visibility = Visibility.Collapsed;
                board.Begin();
            }
        }
    }

    static Button backButton = default!;
    StackPanel titleBar = default!;

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
                titleBar = UIElementProvider.TitleBar(context.Configuration.Get<Configuration>().Apperance.Colors.Accent.Primary, out backButton);
                NavigationView navigationView = UIElementProvider.NavigationView(out Frame contentFrame);
                Grid mainGrid = UIElementProvider.MainGrid(new GridLength[] { new(), new(1, GridUnitType.Star) }, titleBar, navigationView);

                services.AddScoped<INavigation>(provider => new Navigation(navigationView, contentFrame));
                services.AddScoped<IConverter, JsonConverter>();
                services.AddScoped<WindowHandler>();

                services.Configure<Configuration>(context.Configuration);

                services.AddSingleton<ShellViewModel>();
                services.AddSingleton<HomeViewModel>();

                services.AddSingleton<ShellView>(provider => new() { Content = mainGrid });
            })
            .Build();

        Provider = host.Services;


        var logger = Provider.GetRequiredService<ILogger<App>>();

        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            logger.Log("Global FirstChanceException", args.Exception);
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await host.StartAsync();

        var windowHandler = Provider.GetRequiredService<WindowHandler>();
        windowHandler.SetTilteBar(true, titleBar);

        var shellView = Provider.GetRequiredService<ShellView>();
        shellView.Activate();

        var navigation = Provider.GetRequiredService<INavigation>();
        navigation.Navigate("Home");
    }
}