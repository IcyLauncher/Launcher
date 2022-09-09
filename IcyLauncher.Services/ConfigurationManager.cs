namespace IcyLauncher.Services;

public class ConfigurationManager
{
    readonly Configuration configuration;
    readonly ILogger logger;
    readonly IConverter converter;

    public ConfigurationManager(
        IOptions<Configuration> configuration,
        ILogger<ConfigurationManager> logger,
        IConverter converter)
    {
        this.configuration = configuration.Value;
        this.logger = logger;
        this.converter = converter;

        logger.Log("Registered configuration manager");
    }


    public string Export() =>
        converter.ToString(configuration);

    public void Load(
        Configuration input,
        bool ignoreTheme = false)
    {
        if (!ignoreTheme)
            ThemeManager.CopyTheme(configuration.Apperance.Colors, input.Apperance.Colors);

        configuration.Launcher.IsDiscordEnabled = input.Launcher.IsDiscordEnabled;
        configuration.Launcher.IsScrollbarsEnabled = input.Launcher.IsScrollbarsEnabled;
        configuration.Launcher.AtMinecraftaunch = input.Launcher.AtMinecraftaunch;
        configuration.Launcher.TexturepackDirectory = input.Launcher.TexturepackDirectory;
        configuration.Launcher.VersionsDirectory = input.Launcher.VersionsDirectory;

        configuration.Apperance.HomeBanner = input.Apperance.HomeBanner;
        configuration.Apperance.SelectedHomeBanner = input.Apperance.SelectedHomeBanner;
        configuration.Apperance.HomeBannerUri = input.Apperance.HomeBannerUri;
        configuration.Apperance.Backdrop = input.Apperance.Backdrop;
        configuration.Apperance.IsDarkModeBackdropEnabled = input.Apperance.IsDarkModeBackdropEnabled;

        configuration.Weather.IsEnabled = input.Weather.IsEnabled;
        configuration.Weather.IsAutoLocationEnabled = input.Weather.IsAutoLocationEnabled;
        configuration.Weather.Location = input.Weather.Location;
        configuration.Weather.Unit = input.Weather.Unit;

        configuration.DateTime.DateFormat = input.DateTime.DateFormat;
        configuration.DateTime.TimeFormat = input.DateTime.TimeFormat;

        configuration.Developer.IsOneClickEnabled = input.Developer.IsOneClickEnabled;
        configuration.Developer.IsWarningEnabled = input.Developer.IsWarningEnabled;

        logger.Log($"Loaded app configuration from input");
    }
}