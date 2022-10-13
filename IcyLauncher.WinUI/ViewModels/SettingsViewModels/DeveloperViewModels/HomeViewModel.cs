using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.System;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class HomeViewModel : ObservableObject
{
    #region Setup
    readonly ILogger logger;
    readonly IMessage message;

    public readonly Configuration Configuration;
    public readonly IRelayCommand ShowAddButtonFlyoutCommand;

    public HomeViewModel(
        Configuration configuration,
        ILogger logger,
        IMessage message,
        IRelayCommand showAddButtonFlyoutCommand)
    {
        this.logger = logger;
        this.message = message;

        Configuration = configuration;
        ShowAddButtonFlyoutCommand = showAddButtonFlyoutCommand;
    }
    #endregion


    #region GitHUb
    [RelayCommand]
    async Task OpenGitHub()
    {
        if (await message.ShowAsync("Are you sure?", "Do you want to open the IcyLauncher GitHub repo in your default web browser?", closeButton: "No", primaryButton: "Yes") == ContentDialogResult.Primary)
            await Launcher.LaunchUriAsync(new("https://github.com/IcyLauncher/Launcher"));
    }
    #endregion

    #region Garbage Collecter
    [RelayCommand]
    void ForceGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        bool result = Win32.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

        logger.Log($"Forced Garabe Collecter and cleaned application memory [{result}]");
    }
    #endregion

    #region CAT
    [RelayCommand]
    void GenerateRandomCat(Image sender) =>
        sender.Source = "https://thecatapi.com/api/images/get".AsImage(false, BitmapCreateOptions.IgnoreImageCache);
    // other cat api: https://cataas.com/cat/says/IcyLauncher
    #endregion
}