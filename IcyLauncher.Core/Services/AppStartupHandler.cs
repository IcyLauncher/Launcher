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
        AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
            logger.Log("Global Exception thrown", e.Exception, LogLevel.Error, "global", "?");
             
        windowHandler.SetTilteBar(true, uiElementReciever.TitleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(750, 500);
        windowHandler.SetSize(1040, 555);
        windowHandler.SetPositionToCenter();
        windowHandler.MakeTransparent();
        windowHandler.SetBlur(configuration.Value.Apperance.Blur, true, configuration.Value.Apperance.UseDarkModeBlur);

        shell.Closed += async (s, e) => 
            await configurationManagaer.ExportAsync();
        shell.Activate();

        themeManager.SetResourceColors();
        themeManager.SetUnbindableBindings();

        uiElementReciever.BackButton.Click += (s, e) =>
            navigation.GoBack();
        navigation.Navigate("Home");

        logger.Log("App fully startup");
    }
}