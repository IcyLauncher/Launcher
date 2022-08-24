using IcyLauncher.Xaml.Converters;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace IcyLauncher.Services;

public class ThemeManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly IConverter converter;
    readonly UIElementReciever uiElementReciever;
    readonly Random random = new();

    public Theme Colors => configuration.Apperance.Colors;

    public ThemeManager(ILogger<ConfigurationManager> logger, IOptions<Configuration> configuration, IConverter converter, UIElementReciever uiElementReciever)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.converter = converter;
        this.uiElementReciever = uiElementReciever;

        this.configuration.Apperance.Colors.Accent.PropertyChanged += AccentColorsValuesChanged;
        //this.configuration.Apperance.Colors.Background.PropertyChanged += BackgroundColorsValuesChanged;
        //this.configuration.Apperance.Colors.Text.PropertyChanged += TextColorsValuesChanged;
        this.configuration.Apperance.Colors.Control.PropertyChanged += ControlColorsValuesChanged;
        //this.configuration.Apperance.Colors.Control.Solid.PropertyChanged += ControlSolidColorsValuesChanged;
        new UISettings().ColorValuesChanged += SystemColorsValuesChanged;

        this.logger.Log("Registered Theme Manager and hooked all ColorValue changes");
    }


    public string Export() =>
        converter.ToString(configuration.Apperance.Colors);

    public void Load(Theme input, bool ignoreAccent = false)
    {
        CopyTheme(Colors, input, ignoreAccent);

        logger.Log($"Loaded theme configuration from input");
    }

    public static void CopyTheme(Theme copyTo, Theme copyFrom, bool ignoreAccent = false)
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

    /// <summary>
    /// Use only for debugging.
    /// </summary>
    public void RandomizeTheme() =>
        Load(new()
        {
            Accent = new()
            {
                Primary = GetRandomColor(255),
                Light = GetRandomColor(255),
                Dark = GetRandomColor(255)
            },
            Background = new()
            {
                Solid = GetRandomColor(255),
                Transparent = GetRandomColor(180),
                Gradient = GetRandomColor(255),
                GradientTransparent = GetRandomColor(0)
            },
            Text = new()
            {
                Primary = GetRandomColor(255),
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
                    Primary = GetRandomColor(255),
                    Outline = GetRandomColor(255),
                    PrimaryDisabled = GetRandomColor(255),
                    OutlineDisabled = GetRandomColor(255)
                }
            }
        });

    public Color GetRandomColor(byte transparency) =>
        Color.FromArgb(transparency, Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));



    public void SetResourceColors()
    {
        if (Application.Current.Resources["Colors"] is Theme resourceColors)
        {
            resourceColors.Accent = Colors.Accent;
            resourceColors.Background = Colors.Background;
            resourceColors.Text = Colors.Text;
            resourceColors.Control = Colors.Control;
        }

        logger.Log($"Set resource colors to configuration");
    }

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

        logger.Log($"Set unbindable bindings to bindings");
    }


    private void AccentColorsValuesChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Light":
                uiElementReciever.TitleBarIconGradientStops[0].Color = configuration.Apperance.Colors.Accent.Light;

                logger.Log($"Updated TitleBar icon gradient stop (0)");
                break;
            case "Dark":
                uiElementReciever.TitleBarIconGradientStops[1].Color = configuration.Apperance.Colors.Accent.Dark;

                logger.Log($"Updated TitleBar icon gradient stop (1)");
                break;
        }
    }

    private void ControlColorsValuesChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Primary":
                uiElementReciever.CurrentNavigationViewItemLayoutRoot.Background = new SolidColorBrush(configuration.Apperance.Colors.Control.Primary);

                logger.Log($"Updated current NavigationViewItem LayoutRoot");
                break;
        }
    }

    private void SystemColorsValuesChanged(UISettings sender, object args)
    {
        logger.Log("YOUR MOM");
    }
}