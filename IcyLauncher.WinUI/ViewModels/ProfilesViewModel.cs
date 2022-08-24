namespace IcyLauncher.WinUI.ViewModels;

public partial class ProfilesViewModel : ObservableObject
{
    public Configuration Configuration;
    readonly ILogger<ProfilesViewModel> logger;
    readonly IFileSystem fileSystem;

    public ProfilesViewModel(IOptions<Configuration> configuration, ILogger<ProfilesViewModel> logger, IFileSystem fileSystem)
    {
        Configuration = configuration.Value;
        this.logger = logger;
        this.fileSystem = fileSystem;
    }


    [RelayCommand]
    void Test()
    {
    }
}