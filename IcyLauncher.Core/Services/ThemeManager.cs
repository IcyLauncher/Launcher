using Microsoft.Extensions.Options;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace IcyLauncher.Core.Services;

public class ThemeManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly IConverter converter;
    readonly UISettings uiSettings = new();

    public ConfigurationApperanceColors Colors => configuration.Apperance.Colors;

    public ThemeManager(IOptions<Configuration> configuration, ILogger<ConfigurationManager> logger, IConverter converter)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.converter = converter;

        uiSettings.ColorValuesChanged += ColorValuesChanged;

        this.logger.Log("Registered Theme Manager");
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


        logger.Log($"Set Resource Colors to Configuration");
    }


    private void ColorValuesChanged(UISettings sender, object args)
    {
        var accentColor = sender.GetColorValue(UIColorType.Accent);

        var backgroundColor = sender.GetColorValue(UIColorType.Background);
    }
}