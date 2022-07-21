using CommunityToolkit.Mvvm.ComponentModel;

namespace IcyLauncher.Core.Models;

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
    BannerType playBannerType = BannerType.TimeDependent;

    public Theme Colors { get; set; } = new();

    [ObservableProperty]
    BlurEffect blur = BlurEffect.Mica;

    [ObservableProperty]
    bool useDarkModeBlur = true;
}

public partial class ConfigurationWeather : ObservableObject
{
    [ObservableProperty]
    bool isEnabled = true;

    [ObservableProperty]
    string location = "$IP";

    [ObservableProperty]
    WeatherUnit unit = WeatherUnit.Celsius;
}

public partial class ConfigurationDateTime : ObservableObject
{
    [ObservableProperty]
    string dateFormat  = "dd.MM.yyyy";

    [ObservableProperty]
    string timeFormat  = "HH:mm:ss";
}

public partial class ConfigurationDeveloper : ObservableObject
{
    [ObservableProperty]
    bool isSaveLoginEnabled = false;

    [ObservableProperty]
    bool isOneClickEnabled = false;
}