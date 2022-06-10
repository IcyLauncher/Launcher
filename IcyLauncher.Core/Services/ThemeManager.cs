using IcyLauncher.Core.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI.ViewManagement;

namespace IcyLauncher.Core.Services;

public class ThemeManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly IConverter converter;
    readonly ControlReciever controlReciever;

    public ConfigurationApperanceColors Colors => configuration.Apperance.Colors;

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
    public void LoadTheme(ConfigurationApperance input)
    {
        configuration.Apperance = input;
        SetResourceColors();

        logger.Log($"Loaded theme configuration from string");
    }


    public void SetResourceColors()
    {
        if (Application.Current.Resources["Colors"] is ConfigurationApperanceColors resourceColors)
        {
            resourceColors.Accent = configuration.Apperance.Colors.Accent;
            resourceColors.Background = configuration.Apperance.Colors.Background;
            resourceColors.Text = configuration.Apperance.Colors.Text;
            resourceColors.Control = configuration.Apperance.Colors.Control;
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