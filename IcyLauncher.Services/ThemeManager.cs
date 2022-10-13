using IcyLauncher.Xaml.Converters;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Newtonsoft.Json;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace IcyLauncher.Services;

public class ThemeManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly UIElementReciever uiElementReciever;
    readonly BackdropHandler backdropHandler;
    readonly IConverter converter;
    readonly INavigation navigation;

    /// <summary>
    /// The current theme
    /// </summary>
    public Theme Colors => configuration.Apperance.Colors;

    /// <summary>
    /// Manager of the current therme
    /// </summary>
    public ThemeManager(
        ILogger<ConfigurationManager> logger,
        IOptions<Configuration> configuration,
        UIElementReciever uiElementReciever,
        BackdropHandler backdropHandler,
        IConverter converter,
        INavigation navigation)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.uiElementReciever = uiElementReciever;
        this.backdropHandler = backdropHandler;
        this.converter = converter;
        this.navigation = navigation;

        this.configuration.Apperance.Colors.Accent.PropertyChanged += AccentColorsValuesChanged;
        this.configuration.Apperance.Colors.Control.PropertyChanged += ControlColorsValuesChanged;

        this.configuration.Apperance.PropertyChanged += ValuesChanged;

        SubscribeToUISettings(this.configuration.Apperance.UseSystemTheme || this.configuration.Apperance.UseSystemAccent);

        logger.Log("Registered theme manager and hooked all ColorValueChanged events");
    }


    DispatcherQueue? dispatcher;

    readonly UISettings systemUI = new();

    /// <summary>
    /// A boolean wether the app is subscribed to the UISettings
    /// </summary>
    public bool IsSubscribedToUISettings = false;

    /// <summary>
    /// Subscribes the app to all UISettings changes
    /// </summary>
    /// <param name="subscribe">The boolean wether to subscribe or unsubscribe</param>
    public void SubscribeToUISettings(
        bool subscribe)
    {
        if (subscribe == IsSubscribedToUISettings)
            return;

        if (subscribe)
        {
            systemUI.ColorValuesChanged += SystemColorsValuesChanged;
            IsSubscribedToUISettings = true;
            dispatcher = DispatcherQueue.GetForCurrentThread();

            if (configuration.Apperance.UseSystemTheme)
                ValidateTheme();
            if (configuration.Apperance.UseSystemAccent)
                ValidateAccent();
        }
        else
        {
            systemUI.ColorValuesChanged -= SystemColorsValuesChanged;
            IsSubscribedToUISettings = false;
            dispatcher = null;
        }
    }

    bool isDark = true;

    void ValidateTheme()
    {
        bool isDarkModeEnabled = systemUI.GetColorValue(UIColorType.Background) == Microsoft.UI.Colors.Black;

        Load(isDarkModeEnabled ? Theme.Dark : Theme.Light, true);
        configuration.Apperance.IsDarkModeBackdropEnabled = isDarkModeEnabled;

        isDark = isDarkModeEnabled;
    }

    Color accent;

    void ValidateAccent()
    {
        Color currentAccent = systemUI.GetColorValue(UIColorType.Accent);

        Colors.Accent.Primary = currentAccent;
        Colors.Accent.Light = ModifyColor(currentAccent, 1.25);
        Colors.Accent.Dark = ModifyColor(currentAccent, 0.75);

        accent = currentAccent;
    }


    /// <summary>
    /// Exports the current theme as a string
    /// </summary>
    /// <returns>The converted string of the current theme</returns>
    public string Export() =>
        converter.ToString(configuration.Apperance.Colors, Formatting.Indented);

    /// <summary>
    /// Loads a theme as the current theme
    /// </summary>
    /// <param name="input">The theme which should get loaded</param>
    /// <param name="ignoreAccent">The boolean wether the accent should be ignored while loading</param>
    public void Load(
        Theme input,
        bool ignoreAccent = false)
    {
        CopyTheme(Colors, input, ignoreAccent);

        logger.Log($"Loaded theme configuration from input");
    }


    /// <summary>
    /// Copies a theme to another theme
    /// </summary>
    /// <param name="copyTo">The theme which should get copied to</param>
    /// <param name="copyFrom">The theme which should get copied from</param>
    /// <param name="ignoreAccent"></param>
    public static void CopyTheme(
        Theme copyTo,
        Theme copyFrom,
        bool ignoreAccent = false)
    {
        if (!ignoreAccent)
        {
            copyTo.Accent.Primary = copyFrom.Accent.Primary;
            copyTo.Accent.Light = copyFrom.Accent.Light;
            copyTo.Accent.Dark = copyFrom.Accent.Dark;
        }

        copyTo.Background.Solid = copyFrom.Background.Solid;
        copyTo.Background.Transparent = copyFrom.Background.Transparent;
        copyTo.Background.Gradient = copyFrom.Background.Gradient;
        copyTo.Background.GradientTransparent = copyFrom.Background.GradientTransparent;

        copyTo.Text.Primary = copyFrom.Text.Primary;
        copyTo.Text.Secondary = copyFrom.Text.Secondary;
        copyTo.Text.Tertiary = copyFrom.Text.Tertiary;
        copyTo.Text.Disabled = copyFrom.Text.Disabled;

        copyTo.Control.Primary = copyFrom.Control.Primary;
        copyTo.Control.Outline = copyFrom.Control.Outline;
        copyTo.Control.PrimaryDisabled = copyFrom.Control.Outline;
        copyTo.Control.OutlineDisabled = copyFrom.Control.Outline;

        copyTo.Control.Solid.Primary = copyFrom.Control.Solid.Primary;
        copyTo.Control.Solid.Outline = copyFrom.Control.Solid.Outline;
        copyTo.Control.Solid.PrimaryDisabled = copyFrom.Control.Solid.Outline;
        copyTo.Control.Solid.OutlineDisabled = copyFrom.Control.Solid.Outline;
    }


    readonly Random random = new();

    /// <summary>
    /// Randomizes every color of the current theme. Use for debugging only
    /// </summary>
    public void RandomizeTheme() =>
        Load(new()
        {
            Accent = new()
            {
                Primary = GetRandomColor(),
                Light = GetRandomColor(),
                Dark = GetRandomColor()
            },
            Background = new()
            {
                Solid = GetRandomColor(),
                Transparent = GetRandomColor(180),
                Gradient = GetRandomColor(),
                GradientTransparent = GetRandomColor(0)
            },
            Text = new()
            {
                Primary = GetRandomColor(),
                Secondary = GetRandomColor(196),
                Tertiary = GetRandomColor(133),
                Disabled = GetRandomColor(92),
            },
            Control = new()
            {
                Primary = GetRandomColor(13),
                Outline = GetRandomColor(26),
                PrimaryDisabled = GetRandomColor(38),
                OutlineDisabled = GetRandomColor(51),
                Solid = new()
                {
                    Primary = GetRandomColor(),
                    Outline = GetRandomColor(),
                    PrimaryDisabled = GetRandomColor(),
                    OutlineDisabled = GetRandomColor()
                }
            }
        });

    /// <summary>
    /// Generates a random color
    /// </summary>
    /// <param name="transparency">The transparency which the color should have</param>
    /// <returns>The generated color</returns>
    public Color GetRandomColor(byte transparency = 255) =>
        Color.FromArgb(transparency, Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));


    /// <summary>
    /// Sets resource colors from current theme
    /// </summary>
    public void SetResourceColors()
    {
        if (Application.Current.Resources["Colors"] is Theme resourceColors)
        {
            resourceColors.Accent = Colors.Accent;
            resourceColors.Background = Colors.Background;
            resourceColors.Text = Colors.Text;
            resourceColors.Control = Colors.Control;
        }

        logger.Log($"Set resource colors from current theme");
    }

    /// <summary>
    /// Binds non-bindable properties of UIElementReciever elements to their respected colors
    /// </summary>
    public void SetUnbindableBindings()
    {
        ColorBrushConverter converter = new();

        uiElementReciever.BackButton.SetBinding(IconElement.ForegroundProperty, new Binding()
        {
            Source = configuration,
            Converter = converter,
            Path = new PropertyPath("Apperance.Colors.Text.Primary"),
            Mode = BindingMode.OneWay
        });

        uiElementReciever.TitleBarTitle.SetBinding(TextBlock.ForegroundProperty, new Binding()
        {
            Source = configuration,
            Converter = converter,
            Path = new PropertyPath("Apperance.Colors.Accent.Primary"),
            Mode = BindingMode.OneWay
        });

        logger.Log($"Binded unbindable bindings");
    }


    /// <summary>
    /// Darkens/Lightens a color by the given percentage
    /// </summary>
    /// <param name="color">The source of the new color</param>
    /// <param name="darken">Wether the color should be darken or lighten</param>
    /// <param name="percentage">The percentage of the color values that should be darkened/lightened</param>
    /// <returns>The modified Color</returns>
    public static Color ModifyColor(Color color, double percentage)
    {
        byte Calculate(byte source)
        {
            if (source == 0 && percentage != 1) source = 10;
            var res = source * percentage;
            if (res < 0) res = 0;
            if (res > 255) res = 255;
            return Convert.ToByte(res);
        }

        return Color.FromArgb(color.A, Calculate(color.R), Calculate(color.G), Calculate(color.B));
    }


    void AccentColorsValuesChanged(object? _, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Light":
                uiElementReciever.TitleBarIconGradientStops[0].Color = configuration.Apperance.Colors.Accent.Light;

                logger.Log($"Updated TitleBarIconGradientStop [0]");
                break;
            case "Dark":
                uiElementReciever.TitleBarIconGradientStops[1].Color = configuration.Apperance.Colors.Accent.Dark;

                logger.Log($"Updated TitleBarIconGradientStop [1]");
                break;
        }
    }

    void ControlColorsValuesChanged(object? _, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Primary":
                if (navigation.GetCurrentNavigationViewItemLayoutRoot() is Grid layoutRoot)
                    layoutRoot.Background = configuration.Apperance.Colors.Control.Primary.AsSolid();

                logger.Log($"Updated current NavigationLayoutRoot color");
                break;
        }
    }

    void ValuesChanged(object? _, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Backdrop":
                if (backdropHandler.Current != configuration.Apperance.Backdrop)
                    backdropHandler.SetBackdrop(configuration.Apperance.Backdrop, true, configuration.Apperance.IsDarkModeBackdropEnabled);
                break;
            case "IsDarkModeBackdropEnabled":
                backdropHandler.SetDarkMode(configuration.Apperance.Backdrop, configuration.Apperance.IsDarkModeBackdropEnabled);
                break;
            case "UseSystemTheme":
            case "UseSystemAccent":
                SubscribeToUISettings(configuration.Apperance.UseSystemTheme || configuration.Apperance.UseSystemAccent);
                break;
        }
    }

    void SystemColorsValuesChanged(UISettings _, object _1)
    {
        bool isDarkModeEnabled = systemUI.GetColorValue(UIColorType.Background) == Microsoft.UI.Colors.Black;
        if (configuration.Apperance.UseSystemTheme && isDarkModeEnabled != isDark)
        {
            if (dispatcher is not null)
                dispatcher.TryEnqueue(() =>
                {
                    Load(isDarkModeEnabled ? Theme.Dark : Theme.Light, true);
                    configuration.Apperance.IsDarkModeBackdropEnabled = isDarkModeEnabled;
                });

            isDark = isDarkModeEnabled;

            logger.Log($"System color value changed: Theme [{isDarkModeEnabled}]");
        }

        Color currentAccent = systemUI.GetColorValue(UIColorType.Accent);
        if (configuration.Apperance.UseSystemAccent && currentAccent != accent)
        {
            if (dispatcher is not null)
                dispatcher.TryEnqueue(() =>
                {
                    Colors.Accent.Primary = currentAccent;
                    Colors.Accent.Light = ModifyColor(currentAccent, 1.25);
                    Colors.Accent.Dark = ModifyColor(currentAccent, 0.75);
                });

            accent = currentAccent;

            logger.Log($"System color value changed: Accent [{currentAccent}]");
        }
    }
}