using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyLauncher.Core.Services;

public class ThemeManager
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly IConverter converter;

    public ThemeManager(IOptions<Configuration> configuration, ILogger<ConfigurationManager> logger, IConverter converter)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.converter = converter;

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

    }
}