namespace IcyLauncher.ViewModels;

public partial class ProfilesViewModel : ObservableObject
{
    public Configuration Configuration;
    ILogger<ProfilesViewModel> logger;
    IFileSystem fileSystem;

    public ProfilesViewModel(IOptions<Configuration> configuration, ILogger<ProfilesViewModel> logger, IFileSystem fileSystem)
    {
        Configuration = configuration.Value;
        this.logger = logger;
        this.fileSystem = fileSystem;
    }


    [ICommand]
    async void Test()
    {
    }
}