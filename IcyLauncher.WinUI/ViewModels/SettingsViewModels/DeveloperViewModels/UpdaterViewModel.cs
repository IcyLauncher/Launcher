namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class UpdaterViewModel : ObservableObject
{
    #region Setup
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
    #endregion


    #region CurrentAppVersion
    [ObservableProperty]
    Version currentAppVersion = default!;

    [RelayCommand]
    void UpdateCurrentAppVersion() =>
        CurrentAppVersion = updater.CurrentAppVersion;
    #endregion

    #region CurrentApiVersion
    [ObservableProperty]
    Version currentApiVersion = default!;

    [RelayCommand]
    void UpdateCurrentApiVersion() =>
        CurrentApiVersion = updater.CurrentApiVersion;
    #endregion


    #region LastChecked
    [ObservableProperty]
    DateTime lastChecked = default!;

    [RelayCommand]
    void UpdateLastChecked() =>
        LastChecked = updater.LastChecked;
    #endregion

    #region IsUpdateAvailable
    [ObservableProperty]
    bool isUpdateAvailable = default!;

    [RelayCommand]
    void UpdateIsUpdateAvailable() =>
        IsUpdateAvailable = updater.IsUpdateAvailable;
    #endregion
}