namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    void SetupMicaBackdropHandlerViewModel() =>
        MicaBackdropHandler_isDarkModeEnabled = micaBackdropHandler.IsDarkModeEnabled;


    [RelayCommand]
    void MicaBackdropHandler_EnableBackdrop() =>
        micaBackdropHandler.EnableBackdrop();

    [RelayCommand]
    void MicaBackdropHandler_DisableBackdrop() =>
        micaBackdropHandler.DisableBackdrop();


    [ObservableProperty]
    bool micaBackdropHandler_isDarkModeEnabled = default!;

    partial void OnMicaBackdropHandler_isDarkModeEnabledChanged(bool value) =>
        micaBackdropHandler.IsDarkModeEnabled = value;
}