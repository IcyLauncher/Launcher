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

        this.configuration.Apperance.Colors.Accent.PropertyChanged += ColorsValuesChanged;
        //this.configuration.Apperance.Colors.Background.PropertyChanged += ColorsValuesChanged;
        //this.configuration.Apperance.Colors.Text.PropertyChanged += ColorsValuesChanged;
        this.configuration.Apperance.Colors.Control.PropertyChanged += ColorsValuesChanged;
        //this.configuration.Apperance.Colors.Control.Solid.PropertyChanged += ColorsValuesChanged;
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


    private void ColorsValuesChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Light":
                controlReciever.TitleBarIconGradientStops[0].Color = configuration.Apperance.Colors.Accent.Light;
                break;
            case "Dark":
                controlReciever.TitleBarIconGradientStops[1].Color = configuration.Apperance.Colors.Accent.Dark;
                break;
            case "Primary":
                controlReciever.CurrentNavigationViewItemLayoutRoot.Background = new SolidColorBrush(configuration.Apperance.Colors.Control.Primary);
                break;
        }
    }

    private void SystemColorsValuesChanged(UISettings sender, object args)
    {
        //var accentColor = sender.GetColorValue(UIColorType.Accent);
        //var backgroundColor = sender.GetColorValue(UIColorType.Background);
    }
}