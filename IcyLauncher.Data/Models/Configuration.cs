using CommunityToolkit.Mvvm.ComponentModel;

namespace IcyLauncher.Data.Models;

public class Configuration
{
    public ConfigurationLauncher Launcher { get; set; } = new();

    public ConfigurationApperance Apperance { get; set; } = new();

    public ConfigurationWeather Weather { get; set; } = new();

    public ConfigurationDateTime DateTime { get; set; } = new();

    public ConfigurationDeveloper Developer { get; set; } = new();
}

public partial class ConfigurationLauncher : ObservableObject
{
    [ObservableProperty]
    bool isDiscordEnabled = true;

    [ObservableProperty]
    bool isScrollbarsEnabled = true;

    [ObservableProperty]
    MinecraftLaunchBehaviour atMinecraftaunch = MinecraftLaunchBehaviour.Minimize;

    [ObservableProperty]
    string texturepackDirectory = $"{Computer.MinecraftDirectory}\\games\\com.mojang\\resource_packs";

    [ObservableProperty]
    string versionsDirectory = $"{Computer.CurrentDirectory}\\Versions";
}

public partial class ConfigurationApperance : ObservableObject
{
    [ObservableProperty]
    BannerType homeBanner = BannerType.TimeDependent;

    [ObservableProperty]
    int selectedHomeBanner = -1;

    [ObservableProperty]
    string homeBannerUri = "ms-appx:///Assets/Banners/NoBanner.png";

    public Theme Colors { get; set; } = new();

    [ObservableProperty]
    Backdrop backdrop = Backdrop.Mica;

    [ObservableProperty]
    bool isDarkModeBackdropEnabled = true;
}

public partial class ConfigurationWeather : ObservableObject
{
    [ObservableProperty]
    bool isEnabled = true;

    [ObservableProperty]
    bool isAutoLocationEnabled = true;

    [ObservableProperty]
    string location = "New York";

    [ObservableProperty]
    WeatherUnit unit = WeatherUnit.Celsius;
}

public partial class ConfigurationDateTime : ObservableObject
{
    [ObservableProperty]
    string dateFormat  = "MM/dd/yyyy";

    [ObservableProperty]
    string timeFormat  = "hh:mm tt";
}

public partial class ConfigurationDeveloper : ObservableObject
{
    [ObservableProperty]
    bool isDeveloperModeEnabled = false;

    [ObservableProperty]
    bool isOneClickEnabled = false;

    [ObservableProperty]
    bool isWarningEnabled = true;
}