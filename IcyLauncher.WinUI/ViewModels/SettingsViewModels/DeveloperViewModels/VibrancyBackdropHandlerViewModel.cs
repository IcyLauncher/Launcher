namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [RelayCommand]
    void VibrancyBackdropHandler_EnableBackdrop() =>
        vibrancyBackdropHandler.EnableBackdrop();

    [RelayCommand]
    void VibrancyBackdropHandler_DisableBackdrop() =>
        vibrancyBackdropHandler.DisableBackdrop();
}