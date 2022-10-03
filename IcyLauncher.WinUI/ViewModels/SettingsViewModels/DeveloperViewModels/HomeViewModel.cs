using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.System;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class HomeViewModel : ObservableObject
{
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


    [RelayCommand]
    async Task OpenGitHub()
    {
        if (await message.ShowAsync("Are you sure?", "Do you want to open the IcyLauncher GitHub repo in your default web browser?", closeButton: "No", primaryButton: "Yes") == ContentDialogResult.Primary)
            await Launcher.LaunchUriAsync(new("https://github.com/IcyLauncher/Launcher"));
    }


    [RelayCommand]
    void ForceGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        bool result = Win32.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

        logger.Log($"Forced Garabe Collecter and cleaned application memory [{result}]");
    }


    [RelayCommand]
    void GenerateRandomCat(Image sender) =>
        sender.Source = "https://cataas.com/cat/says/IcyLauncher".AsImage(false, BitmapCreateOptions.IgnoreImageCache);
}