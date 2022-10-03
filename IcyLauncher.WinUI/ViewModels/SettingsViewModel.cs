using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly IConverter converter;
    readonly IFileSystem fileSystem;
    readonly INavigation navigation;
    readonly IMessage message;

    public readonly Configuration Configuration;
    public readonly Updater Updater;

    public SettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        ConfigurationManager configurationManager,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        BackdropHandler backdropHandler,
        UIElementReciever uIElementReciever,
        IConverter converter,
        IFileSystem fileSystem,
        Updater updater,
        INavigation navigation,
        IMessage message)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.converter = converter;
        this.fileSystem = fileSystem;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;
        Updater = updater;


        windowHandler.Register(folderPicker);

        windowHandler.Register(savePicker);
        savePicker.FileTypeChoices.Add(new("JSON", new List<string>() { ".json" }));
        savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

        windowHandler.Register(filePicker);
        filePicker.FileTypeFilter.Add(".json");

        IsUpdateVisible = Updater.IsUpdateAvailable;

        Configuration.Apperance.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case "Backdrop":
                    if (backdropHandler.Current != Configuration.Apperance.Backdrop)
                        backdropHandler.SetBackdrop(Configuration.Apperance.Backdrop, true, Configuration.Apperance.IsDarkModeBackdropEnabled);
                    break;
                case "IsDarkModeBackdropEnabled":
                    backdropHandler.SetDarkMode(Configuration.Apperance.Backdrop, Configuration.Apperance.IsDarkModeBackdropEnabled);
                    break;
            }
        };

        Configuration.Developer.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case "UseCustomTitleBar":
                    if (windowHandler.HasCustomTitleBar != Configuration.Developer.UseCustomTitleBar)
                        windowHandler.SetTitleBar(Configuration.Developer.UseCustomTitleBar ? uIElementReciever.TitleBarDragArea : null, uIElementReciever.TitleBarContainer);
                    break;
            }
        };
    }


    int presses = 0;

    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task DebugAsync()
    {
        if (presses >= 5 || Configuration.Developer.IsOneClickEnabled)
        {
            presses = 0;

            if (!Configuration.Developer.IsWarningEnabled || await message.ShowAsync("This might be dangerous!", "Playing around here can be dangerous and ruin your experience with IcyLauncher. It is not recommended to go here.\nDo you really want to continue?", closeButton: "Cancel", primaryButton: "Yes, im a pro") == ContentDialogResult.Primary)
                navigation.SetCurrentPage("Views.SettingsViews.DeveloperSettingsView".AsType());

            return;
        }

        presses++;
        await Task.Delay(1000).ConfigureAwait(false);

        if (presses > 0)
            presses--;
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

            TimeSpan dif = DateTime.Now - Updater.LastChecked;
            UpdateStatus = dif.Hours == 0 ? $"Last checked: {Math.Round(dif.TotalMinutes, 0)} min" : $"Last checked: {Math.Round(dif.TotalHours, 1)} hrs";
            UpdateBadgeVisibility = Visibility.Collapsed;
            UpdateButtonBackground.Color = Colors.Transparent;
            UpdateButtonBorderBrush.Color = Colors.Transparent;
        }
    }

    [RelayCommand]
    void Update()
    {
        IsUpdateVisible = !IsUpdateVisible;
    }


    readonly FolderPicker folderPicker = new() { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };

    [RelayCommand(AllowConcurrentExecutions = false)]
    async Task SelectDirectoryAsync(bool texturepackDirectory)
    {
        StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        if (folder is null || string.IsNullOrWhiteSpace(folder.Path))
            return;

        if (fileSystem.DirectoryExists(folder.Path) && fileSystem.DirectoryWritable(folder.Path))
            if (texturepackDirectory)
                Configuration.Launcher.TexturepackDirectory = folder.Path;
            else
                Configuration.Launcher.VersionsDirectory = folder.Path;
        else
            await message.ShowAsync("Something went wrong :(", "It looks like IcyLauncher cant write to this directory or it does not exist. Please verify that you have given permissions to IcyLauncher and this directory still exists.", closeButton: "Ok");
    }

    [RelayCommand]
    void ResetDirectory(bool texturepackDirectory)
    {
        if (texturepackDirectory)
            Configuration.Launcher.TexturepackDirectory = $"{Computer.MinecraftDirectory}\\games\\com.mojang\\resource_packs";
        else
            Configuration.Launcher.VersionsDirectory = $"{Computer.CurrentDirectory}\\Versions";
    }


    [RelayCommand]
    void NavigateTo(string page) =>
        navigation.SetCurrentPage(page.AsType());


    [RelayCommand(AllowConcurrentExecutions = false)]
    async Task ResetColorsAsync(bool darkMode)
    {
        if (await message.ShowAsync("Are you sure?", $"If you click Ok your current color settings will be overwritten by the default {(darkMode ? "dark" : "light")} mode colors.\nThis will not effect your current accent colors.\nThis will also effect the blur color mode.", primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        themeManager.Load(darkMode ? Theme.Dark : Theme.Light, true);
        Configuration.Apperance.IsDarkModeBackdropEnabled = darkMode;
    }


    public static bool IsDarkModeBackdropEnabled(int selectedIndex) =>
        selectedIndex == 0 || selectedIndex == 1;


    readonly FileSavePicker savePicker = new() { SuggestedStartLocation = PickerLocationId.Desktop };

    [RelayCommand(AllowConcurrentExecutions = false)]
    async Task ExportAsync(bool saveConfig)
    {
        savePicker.SuggestedFileName = saveConfig ? "Config" : "Theme";
        StorageFile save = await savePicker.PickSaveFileAsync();

        if (save is null || string.IsNullOrWhiteSpace(save.Path))
            return;

        if (fileSystem.FileWritable(save.Path))
            await fileSystem.SaveAsTextAsync(save.Path, saveConfig ? configurationManager.Export() : themeManager.Export(), true).ConfigureAwait(false);
        else
            await message.ShowAsync("Something went wrong :(", "It looks like IcyLauncher cant write to this file. Please verify that you have given permissions to IcyLauncher.", closeButton: "Ok");
    }

    readonly FileOpenPicker filePicker = new() { SuggestedStartLocation = PickerLocationId.Desktop };

    [RelayCommand(AllowConcurrentExecutions = false)]
    async Task LoadAsync(bool loadConfig)
    {
        StorageFile file = await filePicker.PickSingleFileAsync();

        if (file is null || string.IsNullOrWhiteSpace(file.Path))
            return;

        if (!fileSystem.FileExists(file.Path))
        {
            await message.ShowAsync("Something went wrong :(", "It looks like this file does no longer exist. Please verify the file still exists.", true, "Ok");
            return;
        }

        string str = loadConfig ? "configuration" : "theme";
        if (await message.ShowAsync("Are you sure?", $"Do you really want to overwrite your current {str} by this external {str}?\nLoading external {str}s can be dangerous. Make sure you backup your current {str}.\nDo you want to continue?", closeButton: "No", primaryButton: "Yes") != ContentDialogResult.Primary)
            return;

        if (loadConfig)
        {
            if (converter.TryToObject(out Configuration? configuration, await fileSystem.ReadAsTextAsync(file.Path).ConfigureAwait(false)) == true)
                configurationManager.Load(configuration!, true);
            else
                await message.ShowAsync("Something went wrong :(", "It looks like this is not a valid configuration.\nPlease verify the file is a proper JSON and every property is being set.", closeButton: "Ok");
        }
        else
        {
            if (converter.TryToObject(out Theme? configuration, await fileSystem.ReadAsTextAsync(file.Path).ConfigureAwait(false)) == true)
                themeManager.Load(configuration!);
            else
                await message.ShowAsync("Something went wrong :(", "It looks like this is not a valid configuration.\nPlease verify the file is a proper JSON and every property is being set.", closeButton: "Ok");
        }
    }
}