namespace IcyLauncher.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    public Configuration Configuration;
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly IFileSystem fileSystem;

    public SettingsViewModel(IOptions<Configuration> configuration, ILogger<ProfilesViewModel> logger, ConfigurationManager configurationManager, ThemeManager themeManager, IFileSystem fileSystem)
    {
        Configuration = configuration.Value;
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.fileSystem = fileSystem;
    }


    [ICommand]
    void Test()
    {

    }
}