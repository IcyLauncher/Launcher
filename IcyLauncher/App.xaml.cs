using IcyLauncher.ViewModels;
using IcyLauncher.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Serilog;

namespace IcyLauncher;

public partial class App : Application
{
    readonly IHost host;
    public static IServiceProvider Provider { get; private set; } = default!;

    static Button backButton = default!;
    StackPanel titleBar = default!;

    readonly Action<bool> CanGoBack = new(CanGoBack =>
    {
        if (CanGoBack == (backButton.Visibility != Visibility.Collapsed))
            return;

        if (CanGoBack)
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
    });

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
                var configuration = context.Configuration.Get<Configuration>();

                titleBar = UIElementProvider.TitleBar(configuration.Apperance.Colors.Accent.Primary, out backButton);
                var navigationView = UIElementProvider.NavigationView(out Frame contentFrame);
                var mainGrid = UIElementProvider.MainGrid(new GridLength[] { new(), new(1, GridUnitType.Star) },
                    configuration.Apperance.Blur == BlurEffect.None ? configuration.Apperance.Colors.Background.Primary : Colors.Transparent, 
                    titleBar, navigationView);

                services.AddScoped<Core.Services.ConfigurationManager>();
                services.AddScoped<INavigation>(provider => new Navigation(provider.GetRequiredService<ILogger<Navigation>>(), navigationView, contentFrame, CanGoBack));
                services.AddScoped<IConverter, JsonConverter>();
                services.AddScoped<WindowHandler>();
                //switch (configuration.Apperance.Blur)
                //{
                //    case BlurEffect.Mica:
                //        services.AddScoped<IBackdropHandler, MicaBackdropHandler>();
                //        break;
                //    case BlurEffect.Acrylic:
                //        services.AddScoped<IBackdropHandler, AcrylicBackdropHandler>();
                //        break;
                //}

                services.Configure<Configuration>(context.Configuration);

                services.AddSingleton<ShellViewModel>();
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


        var windowHandler = Provider.GetRequiredService<WindowHandler>();
        windowHandler.SetTilteBar(true, titleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(700, 400);
        windowHandler.SetSize(1031, 550);
        windowHandler.SetPositionToCenter();
        windowHandler.MakeTransparent();

        var shellView = Provider.GetRequiredService<Window>();
        shellView.Activate();

        //var backdropHandler = Provider.GetService<IBackdropHandler>();
        //if (backdropHandler is not null) 
        //    backdropHandler.SetBackdrop();

        var navigation = Provider.GetRequiredService<INavigation>();
        navigation.Navigate("Home");
        backButton.Click += (s, e) => navigation.GoBack();

        var logger = Provider.GetRequiredService<ILogger<App>>();
        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            logger.Log("Global FirstChanceException", args.Exception);
    }
}