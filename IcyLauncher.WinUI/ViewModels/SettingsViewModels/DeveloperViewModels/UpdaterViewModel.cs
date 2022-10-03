namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class UpdaterViewModel : ObservableObject
{
    readonly Updater updater;

    public UpdaterViewModel(
        Updater updater)
    {
        this.updater = updater;


        UpdateCurrentAppVersion();
        UpdateCurrentApiVersion();
        UpdateLastChecked();
        UpdateIsUpdateAvailable();
    }


    [ObservableProperty]
    Version currentAppVersion = default!;

    [RelayCommand]
    void UpdateCurrentAppVersion() =>
        CurrentAppVersion = updater.CurrentAppVersion;


    [ObservableProperty]
    Version currentApiVersion = default!;

    [RelayCommand]
    void UpdateCurrentApiVersion() =>
        CurrentApiVersion = updater.CurrentApiVersion;


    [ObservableProperty]
    DateTime lastChecked = default!;

    [RelayCommand]
    void UpdateLastChecked() =>
        LastChecked = updater.LastChecked;


    [ObservableProperty]
    bool isUpdateAvailable = default!;

    [RelayCommand]
    void UpdateIsUpdateAvailable() =>
        IsUpdateAvailable = updater.IsUpdateAvailable;
}