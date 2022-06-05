using Microsoft.Extensions.Options;

namespace IcyLauncher.Core.Services;

public class ConfigurationManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly IConverter converter;

    public ConfigurationManager(IOptions<Configuration> configuration, ILogger<ConfigurationManager> logger, IConverter converter)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.converter = converter;

        this.logger.Log("Registered ConfigurationManager");
    }

    public string Export()
    {
        logger.Log($"Exporting app configuration");

        return converter.ToString(configuration);
    }
    public void Load(Configuration input, bool IgnoreTheme = false)
    {
        configuration.Launcher = input.Launcher;
        if (!IgnoreTheme)
            configuration.Apperance = input.Apperance;
        configuration.Weather = input.Weather;
        configuration.DateTime = input.DateTime;
        configuration.Developer = input.Developer;

        logger.Log($"Loaded app configuration from string");
    }

    public string ExportTheme()
    {
        logger.Log($"Exporting theme configuration");

        return converter.ToString(configuration.Apperance);
    }
    public void LoadTheme(ConfigurationApperance input)
    {
        configuration.Apperance = input;

        logger.Log($"Loaded theme configuration from string");
    }
}