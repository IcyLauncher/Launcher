namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    void SetupAcrylicBackdropHandlerViewModel() =>
        AcrylicBackdropHandler_isDarkModeEnabled = acrylicBackdropHandler.IsDarkModeEnabled;


    [RelayCommand]
    void AcrylicBackdropHandler_EnableBackdrop() =>
        acrylicBackdropHandler.EnableBackdrop();

    [RelayCommand]
    void AcrylicBackdropHandler_DisableBackdrop() =>
        acrylicBackdropHandler.DisableBackdrop();


    [ObservableProperty]
    bool acrylicBackdropHandler_isDarkModeEnabled = default!;

    partial void OnAcrylicBackdropHandler_isDarkModeEnabledChanged(bool value) =>
        acrylicBackdropHandler.IsDarkModeEnabled = value;
}