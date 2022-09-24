namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [RelayCommand]
    void ThemeManager_RandomizeTheme() =>
        themeManager.RandomizeTheme();

    [RelayCommand]
    void ThemeManager_SetResourceColors() =>
        themeManager.SetResourceColors();

    [RelayCommand]
    void ThemeManager_SetUnbindableBindings() =>
        themeManager.SetUnbindableBindings();
}