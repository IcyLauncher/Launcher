using IcyLauncher.Core.Xaml;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media ;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.Core.Services;

public class AppStartupHandler
{
    public AppStartupHandler(
        IOptions<Configuration> configuration,
        ILogger<AppStartupHandler> logger,
        ConfigurationManager configurationManagaer,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        Window shell,
        INavigation navigation)
    {
        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            logger.Log("Global FirstChanceException", args.Exception);

        var titleBar = (StackPanel)((Grid)shell.Content).Children[0];
        var backButton = (Button)titleBar.Children[0];
        var titleTextBlock = (TextBlock)titleBar.Children[2];

        titleTextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding()
        {
            Source = configuration.Value,
            Converter = new BrushConverter(),
            Path = new PropertyPath("Apperance.Colors.Accent.Primary"),
            Mode = BindingMode.OneWay
        });

        var iconColorStops = ((LinearGradientBrush)((Path)((Viewbox)titleBar.Children[1]).Child).Fill).GradientStops;
        configuration.Value.Apperance.Colors.Accent.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == "Light")
            {
                iconColorStops[0].Color = configuration.Value.Apperance.Colors.Accent.Light;
            }
            if (e.PropertyName == "Dark")
            {
                iconColorStops[1].Color = configuration.Value.Apperance.Colors.Accent.Dark;
            }
        };
        Application.Current.Resources["SystemAccentColorLight2"] = configuration.Value.Apperance.Colors.Accent.Light;
        Application.Current.Resources["SystemAccentColorDark1"] = configuration.Value.Apperance.Colors.Accent.Light;

        windowHandler.SetTilteBar(true, titleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(700, 400);
        windowHandler.SetSize(1031, 550);
        windowHandler.SetPositionToCenter();
        windowHandler.MakeTransparent();
        windowHandler.SetBlur(configuration.Value.Apperance.Blur, true);

        shell.Closed += (s, e) => logger.Log(configurationManagaer.Export());
        shell.Activate();

        themeManager.SetResourceColors();

        backButton.Click += (s, e) => navigation.GoBack();
        navigation.Navigate("Home");

        logger.Log("App fully startup");
    }
}