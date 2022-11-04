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
        WindowHandler windowHandler,
        BackdropHandler backdropHandler,
        IConverter converter,
        IFileSystem fileSystem,
        INavigation navigation,
        CoreWindow shell)
    {
        AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
        {
            logger.Log("Global exception thrown", e.Exception, LogLevel.Error, "global", "?");

            //if (e.Exception.Source != "CommunityToolkit.WinUI.UI" || e.Exception is OperationCanceledException)
            //    await message.ShowAsync("Somethig went wrong :(", $"It looks like something bad just happend. An unhandled exception just threw ({e.Exception.Source}).\nWe are sorry that this just happend. You can get support on the official IcyCord-Discord Server or you can report the crash log to IcyLauncher directly.", primaryButton: "Join IcyCord", secondaryButton: "Report Crash");
            //    Process.Start(Crash Window);
        };

        if (solidColors.Value.Container is null)
            solidColors.Value.Container = new(SolidColorCollection.Default);

        if (configuration.Value.Developer.UseCustomTitleBar)
            windowHandler.SetTitleBar(shell.TitleBarDragArea, shell.TitleBarContainer);
        windowHandler.SetIcon("Assets/Icon.ico");
        windowHandler.SetMinSize(750, 500);
        windowHandler.SetSize(1040, windowHandler.HasCustomTitleBar ? 556 : 538);
        windowHandler.SetPositionToCenter();

        backdropHandler.SetBackdrop(configuration.Value.Apperance.Backdrop, true, configuration.Value.Apperance.IsDarkModeBackdropEnabled);

        shell.BackButton.Click += (s, e) =>
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