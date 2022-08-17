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
    readonly ILogger<ProfilesViewModel> logger;
    readonly ThemeManager themeManager;
    readonly IMessage message;

    public readonly Configuration Configuration;

    public ColorSettingsViewModel(IOptions<Configuration> configuration,
        ILogger<ProfilesViewModel> logger,
        ThemeManager themeManager,
        IMessage message)
    {
        this.logger = logger;
        this.message = message;
        this.themeManager = themeManager;

        Configuration = configuration.Value;


        ThemeManager.CopyTheme(LocalColors, themeManager.Colors);
    }


    public Theme LocalColors = new();


    [ICommand]
    void SaveColors() =>
        themeManager.LoadTheme(LocalColors);

    [ICommand(AllowConcurrentExecutions = false)]
    async Task ResetColorsAsync(int darkMode_)
    {
        bool darkMode = darkMode_ == 1;

        if (await message.ShowAsync("Are you sure?", $"If you click Ok your current color settings will be overwritten to the default {(darkMode ? "dark" : "light")} mode colors.\nThis will not effect the blur color mode. To change the blur color mode, change 'Application Background' in the main settings.", true, primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        ThemeManager.CopyTheme(LocalColors, darkMode ? Theme.Dark : Theme.Light);
    }
}