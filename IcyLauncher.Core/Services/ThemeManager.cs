using IcyLauncher.Core.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace IcyLauncher.Core.Services;

public class ThemeManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly IConverter converter;
    readonly ControlReciever controlReciever;

    public Theme Colors => configuration.Apperance.Colors;

    public ThemeManager(ILogger<ConfigurationManager> logger, IOptions<Configuration> configuration, IConverter converter, ControlReciever controlReciever)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.converter = converter;
        this.controlReciever = controlReciever;

        this.configuration.Apperance.Colors.Accent.PropertyChanged += AccentColorsValuesChanged;
        //this.configuration.Apperance.Colors.Background.PropertyChanged += BackgroundColorsValuesChanged;
        //this.configuration.Apperance.Colors.Text.PropertyChanged += TextColorsValuesChanged;
        this.configuration.Apperance.Colors.Control.PropertyChanged += ControlColorsValuesChanged;
        //this.configuration.Apperance.Colors.Control.Solid.PropertyChanged += ControlSolidColorsValuesChanged;
        new UISettings().ColorValuesChanged += SystemColorsValuesChanged;

        this.logger.Log("Registered Theme Manager and hooked all ColorValue changes");
    }

    public string ExportTheme()
    {
        logger.Log($"Exporting theme configuration");

        return converter.ToString(configuration.Apperance);
    }
    public void LoadTheme(Theme input, bool ignoreAccent = false)
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
        var brushConverter = new BrushConverter();

        controlReciever.BackButton.SetBinding(IconElement.ForegroundProperty, new Binding()
        {
            Source = configuration,
            Converter = brushConverter,
            Path = new PropertyPath("Apperance.Colors.Text.Primary"),
            Mode = BindingMode.OneWay
        });

        controlReciever.TitleBarTitle.SetBinding(TextBlock.ForegroundProperty, new Binding()
        {
            Source = configuration,
            Converter = brushConverter,
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
                controlReciever.TitleBarIconGradientStops[0].Color = configuration.Apperance.Colors.Accent.Light;

                logger.Log($"Updated TitleBar icon gradient stop (0)");
                break;
            case "Dark":
                controlReciever.TitleBarIconGradientStops[1].Color = configuration.Apperance.Colors.Accent.Dark;

                logger.Log($"Updated TitleBar icon gradient stop (1)");
                break;
        }
    }

    private void ControlColorsValuesChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Primary":
                controlReciever.CurrentNavigationViewItemLayoutRoot.Background = new SolidColorBrush(configuration.Apperance.Colors.Control.Primary);

                logger.Log($"Updated current NavigationViewItem LayoutRoot");
                break;
        }
    }

    private void SystemColorsValuesChanged(UISettings sender, object args)
    {
        //var accentColor = sender.GetColorValue(UIColorType.Accent);
        //var backgroundColor = sender.GetColorValue(UIColorType.Background);
    }
}