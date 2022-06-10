namespace IcyLauncher.Core.Services;

public class AppStartupHandler
{
    public AppStartupHandler(
        IOptions<Configuration> configuration,
        ILogger<AppStartupHandler> logger,
        ConfigurationManager configurationManagaer,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        ControlReciever controlReciever,
        Window shell,
        INavigation navigation)
    {
        AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            logger.Log("Global Exception thrown", args.Exception);
             
        windowHandler.SetTilteBar(true, controlReciever.TitleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(700, 400);
        windowHandler.SetSize(1031, 700);//550);
        windowHandler.SetPositionToCenter();
        windowHandler.MakeTransparent();
        windowHandler.SetBlur(configuration.Value.Apperance.Blur, true);

        shell.Closed += (s, e) => logger.Log(configurationManagaer.Export());
        shell.Activate();

        themeManager.SetResourceColors();
        themeManager.SetUnbindableBindings();

        controlReciever.BackButton.Click += (s, e) => navigation.GoBack();
        navigation.Navigate("Home");

        logger.Log("App fully startup");
    }
}