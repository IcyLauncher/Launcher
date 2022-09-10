﻿namespace IcyLauncher.WinUI.ViewModels;

public partial class ColorSettingsViewModel : ObservableObject
{
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


        ThemeManager.CopyTheme(LocalColors, themeManager.Colors);
    }

    public Theme LocalColors = new();


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);


    [RelayCommand]
    void SaveColors() =>
        themeManager.Load(LocalColors);

    [RelayCommand(AllowConcurrentExecutions = false)]
    async Task ResetColorsAsync(bool darkMode)
    {
        if (await message.ShowAsync("Are you sure?", $"If you click Ok your current color settings will be overwritten by the default {(darkMode ? "dark" : "light")} mode colors.\nThis will also effect your current accent colors.\nThis will not effect the blur color mode. To change the blur color mode, change 'Application Background' in the main settings.", primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        ThemeManager.CopyTheme(LocalColors, darkMode ? Theme.Dark : Theme.Light);
    }
}