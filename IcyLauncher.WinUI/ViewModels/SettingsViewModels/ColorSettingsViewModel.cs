namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels;

public partial class ColorSettingsViewModel : ObservableObject
{
    #region Setup
    readonly ThemeManager themeManager;
    readonly INavigation navigation;
    readonly IMessage message;

    public readonly Configuration Configuration;

    public ColorSettingsViewModel(
        IOptions<Configuration> configuration,
        ThemeManager themeManager,
        INavigation navigation,
        IMessage message)
    {
        this.themeManager = themeManager;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;

        SetupViewModel();
    }

    void SetupViewModel()
    {
        ThemeManager.CopyTheme(LocalColors, themeManager.Colors);
    }

    public Theme LocalColors = new();
    #endregion


    #region Navigation
    [RelayCommand]
    void SetNavigationIndex() =>
        navigation.SetCurrentIndex(5);
    #endregion


    #region Actions
    [RelayCommand]
    void SaveColors() =>
        themeManager.Load(LocalColors);

    [RelayCommand]
    async Task ResetColorsAsync(bool darkMode)
    {
        if (await message.ShowAsync("Are you sure?", $"If you click Ok your current color settings will be overwritten by the default {(darkMode ? "dark" : "light")} mode colors.\nThis will also effect your current accent colors.\nThis will not effect the blur color mode. To change the blur color mode, change 'Application Backdrop' in the main settings.", primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        ThemeManager.CopyTheme(LocalColors, darkMode ? Theme.Dark : Theme.Light);
        SaveColors();
    }
    #endregion
}