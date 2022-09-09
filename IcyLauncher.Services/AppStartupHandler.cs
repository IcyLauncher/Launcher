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
        BackdropHandler backdropHandler,
        UIElementReciever uiElementReciever,
        Window shell,
        IConverter converter,
        IFileSystem fileSystem,
        INavigation navigation)
    {
        AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
        {
            //e.Exception.Source != "CommunityToolkit.WinUI.UI" || e.Exception is OperationCanceledException
            logger.Log("Global exception thrown", e.Exception, LogLevel.Error, "global", "?");
        };

        bool customTitleBar = windowHandler.SetTitleBar(true, uiElementReciever.TitleBar);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(750, 500);
        windowHandler.SetSize(1040, customTitleBar ? 555 : 538);
        windowHandler.SetPositionToCenter();

        backdropHandler.SetBackdrop(configuration.Value.Apperance.Backdrop, true, configuration.Value.Apperance.IsDarkModeBackdropEnabled);

        shell.Closed += async (s, e) =>
        {
            await fileSystem.SaveAsTextAsync("Configuration.json", configurationManagaer.Export(), true).ConfigureAwait(false);

            await fileSystem.SaveAsTextAsync("Assets\\Banners\\SolidColors.json", converter.ToString(solidColors.Value), true).ConfigureAwait(false);
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