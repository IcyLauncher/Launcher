namespace IcyLauncher.Services;

public class AppStartupHandler
{
    /// <summary>
    /// Handler which configures the entire application on startup 
    /// </summary>
    public AppStartupHandler(
        ILogger<AppStartupHandler> logger,
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ConfigurationManager configurationManagaer,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        UIElementReciever uiElementReciever,
        BackdropHandler backdropHandler,
        IConverter converter,
        IFileSystem fileSystem,
        INavigation navigation,
        Window shell)
    {
        AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
        {
            logger.Log("Global exception thrown", e.Exception, LogLevel.Error, "global", "?");

            //e.Exception.Source != "CommunityToolkit.WinUI.UI" || e.Exception is OperationCanceledException
            // => Show message popup
        };

        if (solidColors.Value.Container is null)
            solidColors.Value.Container = new(SolidColorCollection.Default);

        themeManager.SetResourceColors();
        themeManager.SetUnbindableBindings();

        if (configuration.Value.Developer.UseCustomTitleBar)
            windowHandler.SetTitleBar(uiElementReciever.TitleBarDragArea, uiElementReciever.TitleBarContainer);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(750, 500);
        windowHandler.SetSize(1040, windowHandler.HasCustomTitleBar ? 556 : 538);
        windowHandler.SetPositionToCenter();

        backdropHandler.SetBackdrop(configuration.Value.Apperance.Backdrop, true, configuration.Value.Apperance.IsDarkModeBackdropEnabled);

        uiElementReciever.BackButton.Click += (s, e) =>
            navigation.GoBack();
        navigation.Navigate("Home");

        shell.Closed += async (s, e) =>
        {
            if (windowHandler.LoggerWindow is not null)
                windowHandler.LoggerWindow.Close();
           
            await fileSystem.SaveAsTextAsync("Configuration.json", configurationManagaer.Export(), true).ConfigureAwait(false);
            await fileSystem.SaveAsTextAsync("Assets\\Banners\\SolidColors.json", converter.ToString(solidColors.Value), true).ConfigureAwait(false);
        };
        shell.Activate();

        logger.Log("App fully startup");
    }
}