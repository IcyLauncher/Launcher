using CommunityToolkit.WinUI.UI;

namespace IcyLauncher.Core.Services;

public class AppStartupHandler
{
    public AppStartupHandler(
        IOptions<Configuration> configuration,
        ILogger<AppStartupHandler> logger,
        ConfigurationManager configurationManagaer,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        UIElementReciever uiElementReciever,
        Window shell,
        INavigation navigation)
    {
        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            logger.Log("Global Exception thrown", args.Exception);
             
        windowHandler.SetTilteBar(true, uiElementReciever.TitleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(700, 400);
        windowHandler.SetSize(1031, 550);
        windowHandler.SetPositionToCenter();
        windowHandler.MakeTransparent();
        windowHandler.SetBlur(configuration.Value.Apperance.Blur, true, configuration.Value.Apperance.UseDarkModeBlur);

        shell.Closed += (s, e) => logger.Log(configurationManagaer.Export());
        shell.Activate();

        themeManager.SetResourceColors();
        themeManager.SetUnbindableBindings();

        uiElementReciever.BackButton.Click += (s, e) => navigation.GoBack();
        navigation.Navigate("Home");

        logger.Log("App fully startup");
    }
}