using CommunityToolkit.Mvvm.ComponentModel;
using Windows.UI;

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
    MinecraftLaunchBehaviour atMinecraftaunch = MinecraftLaunchBehaviour.KeppOpen;

    [ObservableProperty]
    string texturepackDirectory = $"{Computer.MinecraftDirectory}\\games\\com.mojang\\resource_packs";

    [ObservableProperty]
    string versionsDirectory = $"{Computer.CurrentDirectory}\\Versions";
}

public partial class ConfigurationApperance : ObservableObject
{
    [ObservableProperty]
    BannerType playBannerType = BannerType.TimeDependent;

    public ConfigurationApperanceColors Colors { get; set; } = new();

    [ObservableProperty]
    BlurEffect blur = BlurEffect.Mica;
}
public class ConfigurationApperanceColors
{
    public ConfigurationApperanceColorsAccent Accent { get; set; } = new();

    public ConfigurationApperanceColorsBackground Background { get; set; } = new();

    public ConfigurationApperanceColorsText Text { get; set; } = new();

    public ConfigurationApperanceColorsControl Control { get; set; } = new();
}
public partial class ConfigurationApperanceColorsAccent : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 0, 138, 255);

    [ObservableProperty]
    Color light = Color.FromArgb(255, 125, 195, 255);

    [ObservableProperty]
    Color dark = Color.FromArgb(255, 0, 73, 135);
}
public partial class ConfigurationApperanceColorsBackground : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 32, 32, 32);

    [ObservableProperty]
    Color light = Color.FromArgb(255, 58, 58, 58);

    [ObservableProperty]
    Color dark = Color.FromArgb(255, 23, 23, 23);
}
public partial class ConfigurationApperanceColorsText : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 255, 255, 255);

    [ObservableProperty]
    Color secondary = Color.FromArgb(196, 255, 255, 255);

    [ObservableProperty]
    Color tertiary = Color.FromArgb(133, 255, 255, 255);

    [ObservableProperty]
    Color disabled = Color.FromArgb(92, 255, 255, 255);
}
public partial class ConfigurationApperanceColorsControl : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(13, 255, 255, 255);

    [ObservableProperty]
    Color outline = Color.FromArgb(26, 255, 255, 255);

    [ObservableProperty]
    Color primaryDisabled = Color.FromArgb(38, 255, 255, 255);

    [ObservableProperty]
    Color outlineDisabled = Color.FromArgb(51, 255, 255, 255);

    public ConfigurationApperanceColorsControlSolid Solid { get; set; } = new();
}
public partial class ConfigurationApperanceColorsControlSolid : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 46, 46, 46);

    [ObservableProperty]
    Color outline = Color.FromArgb(255, 56, 56, 56);

    [ObservableProperty]
    Color primaryDisabled = Color.FromArgb(255, 67, 67, 67);

    [ObservableProperty]
    Color outlineDisabled = Color.FromArgb(255, 79, 79, 79);
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