using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Core.Services;

public class ConfigurationManager
{
    readonly Configuration configuration;
    readonly ILogger logger;
    readonly IConverter converter;

    public ConfigurationManager(IOptions<Configuration> configuration, ILogger<ConfigurationManager> logger, IConverter converter)
    {
        this.configuration = configuration.Value;
        this.logger = logger;
        this.converter = converter;

        this.logger.Log("Registered Configuration Manager");
    }


    public string Export() =>
        converter.ToString(configuration);

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
}