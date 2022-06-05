using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Core.Services;

public class AppStartupHandler
{
    public AppStartupHandler(
        IOptions<Configuration> configuration,
        ILogger<AppStartupHandler> logger,
        WindowHandler windowHandler,
        Window shell,
        INavigation navigation)
    {
        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            logger.Log("Global FirstChanceException", args.Exception);

        var mainGrid = (Grid)shell.Content;
        var titleBar = (StackPanel)mainGrid.Children[0];
        var backButton = (Button)titleBar.Children[0];

        windowHandler.SetTilteBar(true, titleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(700, 400);
        windowHandler.SetSize(1031, 550);
        windowHandler.SetPositionToCenter();
        windowHandler.MakeTransparent();
        windowHandler.SetBlur(configuration.Value.Apperance.Blur, true);

        shell.Activate();

        backButton.Click += (s, e) => navigation.GoBack();
        navigation.Navigate("Home");

        logger.Log("App fully startup");
    }
}