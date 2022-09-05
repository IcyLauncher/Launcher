namespace IcyLauncher.Services;

public class AppStartupHandler
{
    public AppStartupHandler(
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ILogger<AppStartupHandler> logger,
        ConfigurationManager configurationManagaer,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        UIElementReciever uiElementReciever,
        Window shell,
        IConverter converter,
        IFileSystem fileSystem,
        INavigation navigation)
    {
        AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
        {
            //e.Exception.Source != "CommunityToolkit.WinUI.UI"
            logger.Log("Global exception thrown", e.Exception, LogLevel.Error, "global", "?");
        };

        bool customTitleBar = windowHandler.SetTitleBar(true, uiElementReciever.TitleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(750, 500);
        windowHandler.SetSize(1040, customTitleBar ? 555 : 538);
        windowHandler.SetPositionToCenter();
        //windowHandler.MakeTransparent();
        //windowHandler.SetBlur(configuration.Value.Apperance.Blur, true, configuration.Value.Apperance.UseDarkModeBlur);

        shell.Closed += async (s, e) =>
        {
            await fileSystem.SaveAsTextAsync("Configuration.json", configurationManagaer.Export(), true);

            await fileSystem.SaveAsTextAsync("Assets\\Banners\\SolidColors.json", converter.ToString(solidColors.Value), true);
        };
        shell.Activate();

        themeManager.SetResourceColors();
        themeManager.SetUnbindableBindings();

        if (solidColors.Value.Container is null)
            solidColors.Value.Container = new(SolidColorCollection.Default);

        uiElementReciever.BackButton.Click += (s, e) =>
            navigation.GoBack();
        navigation.Navigate("Home");

        logger.Log("App fully startup");
    }
}