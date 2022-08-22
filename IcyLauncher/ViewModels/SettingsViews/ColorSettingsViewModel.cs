using IcyLauncher.UI;
using IcyLauncher.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class ColorSettingsViewModel : ObservableObject
{
    readonly ThemeManager themeManager;
    readonly IMessage message;
    readonly INavigation navigation;

    public readonly Configuration Configuration;

    public ColorSettingsViewModel(IOptions<Configuration> configuration,
        ThemeManager themeManager,
        IMessage message,
        INavigation navigation)
    {
        this.message = message;
        this.themeManager = themeManager;
        this.navigation = navigation;

        Configuration = configuration.Value;


        ThemeManager.CopyTheme(LocalColors, themeManager.Colors);
    }

    public Theme LocalColors = new();


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);


    [ICommand]
    void SaveColors() =>
        themeManager.LoadTheme(LocalColors);

    [ICommand(AllowConcurrentExecutions = false)]
    async Task ResetColorsAsync(bool darkMode)
    {
        if (await message.ShowAsync("Are you sure?", $"If you click Ok your current color settings will be overwritten by the default {(darkMode ? "dark" : "light")} mode colors.\nThis will also effect your current accent colors.\nThis will not effect the blur color mode. To change the blur color mode, change 'Application Background' in the main settings.", true, primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        ThemeManager.CopyTheme(LocalColors, darkMode ? Theme.Dark : Theme.Light);
    }
}