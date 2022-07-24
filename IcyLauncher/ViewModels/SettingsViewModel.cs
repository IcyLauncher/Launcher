using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel.DataAnnotations;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly IFileSystem fileSystem;
    readonly IMessage message;
    readonly INavigation navigation;

    public readonly Configuration Configuration;
    public readonly Updater Updater;

    public SettingsViewModel(IOptions<Configuration> configuration,
        ILogger<ProfilesViewModel> logger,
        ConfigurationManager configurationManager,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        Updater updater,
        IFileSystem fileSystem,
        IMessage message,
        INavigation navigation)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.fileSystem = fileSystem;
        this.message = message;
        this.navigation = navigation;

        Configuration = configuration.Value;
        Updater = updater;


        this.windowHandler.Register(folderPicker);

        Configuration.Apperance.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case "Blur":
                    if (windowHandler.CurrentBlur != Configuration.Apperance.Blur)
                        windowHandler.SetBlur(Configuration.Apperance.Blur, true, Configuration.Apperance.UseDarkModeBlur);
                    break;
                case "UseDarkModeBlur":
                    windowHandler.SetBlur(Configuration.Apperance.Blur, true, Configuration.Apperance.UseDarkModeBlur);
                    break;
            }
        };
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1)
    {
    }


    [ICommand(AllowConcurrentExecutions = true)]
    async Task Debug()
    {
        await message.ShowAsync("Title", "content", false, "No", "Yes");
        await Task.Delay(1000);
        await message.ShowAsync("Title", "content2", false, "No", "Yes");
    }


    [ObservableProperty]
    string updateStatus = "Last checked: 0 min";

    [ObservableProperty]
    Visibility updateBadgeVisibility = Visibility.Collapsed;

    public SolidColorBrush UpdateButtonBackground = new(Colors.Transparent);
    public SolidColorBrush UpdateButtonBorderBrush = new(Colors.Transparent);

    bool isUpdateVisible = false;
    public bool IsUpdateVisible
    {
        get => isUpdateVisible;
        set
        {
            if (isUpdateVisible == value)
                return;

            isUpdateVisible = value;
            logger.Log("Updating update visibility");

            if (value)
            {
                UpdateStatus = "New update available!";
                UpdateBadgeVisibility = Visibility.Visible;
                UpdateButtonBackground.Color = Color.FromArgb(20, 255, 255, 0);
                UpdateButtonBorderBrush.Color = Color.FromArgb(50, 255, 255, 0);
                return;
            }

            var dif = DateTime.Now - Updater.LastChecked;
            UpdateStatus = dif.Hours == 0 ? $"Last checked: {Math.Round(dif.TotalMinutes, 0)} min" : $"Last checked: {Math.Round(dif.TotalHours, 1)} hrs";
            UpdateBadgeVisibility = Visibility.Collapsed;
            UpdateButtonBackground.Color = Colors.Transparent;
            UpdateButtonBorderBrush.Color = Colors.Transparent;
        }
    }

    [ICommand]
    void Update()
    {
        IsUpdateVisible = !IsUpdateVisible;
    }


    readonly FolderPicker folderPicker = new() { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };

    [ICommand(AllowConcurrentExecutions = false)]
    async Task SelectDirectory(int directory)
    {
        var folder = await folderPicker.PickSingleFolderAsync();

        if (folder is null || string.IsNullOrWhiteSpace(folder.Path))
            return;

        if (fileSystem.DirectoryExists(folder.Path) && fileSystem.DirectoryWritable(folder.Path))
            switch (directory)
            {
                case 0:
                    Configuration.Launcher.TexturepackDirectory = folder.Path;
                    break;
                case 1:
                    Configuration.Launcher.VersionsDirectory = folder.Path;
                    break;
            }
        else
            await message.ShowAsync("Something went wrong :(", "It looks like IcyLauncher cant write to this directory or it does not exist. Please verify that you have given permissions to IcyLauncher and this directory still exists.", true, "Ok");
    }

    [ICommand]
    void ResetDirectory(int directory)
    {
        switch (directory)
        {
            case 0:
                Configuration.Launcher.TexturepackDirectory = $"{Computer.MinecraftDirectory}\\games\\com.mojang\\resource_packs";
                break;
            case 1:
                Configuration.Launcher.VersionsDirectory = $"{Computer.CurrentDirectory}\\Versions";
                break;
        }
    }


    [ICommand]
    void NavigateTo(string page) =>
        navigation.SetCurrentPage(page.AsType());


    public bool IsUseBlurDarkModeEnabled(int selectedIndex) =>
        selectedIndex == 0 || selectedIndex == 1;
}