using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    #region Setup
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly UIElementReciever uIElementReciever;
    readonly IConverter converter;
    readonly FeedbackRequest feedbackRequest;
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
        UIElementReciever uIElementReciever,
        IConverter converter,
        FeedbackRequest feedbackRequest,
        IFileSystem fileSystem,
        Updater updater,
        INavigation navigation,
        IMessage message)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.uIElementReciever = uIElementReciever;
        this.converter = converter;
        this.feedbackRequest = feedbackRequest;
        this.fileSystem = fileSystem;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;
        Updater = updater;

        SetupViewModel();
    }

    void SetupViewModel()
    {
        windowHandler.Register(folderPicker);

        windowHandler.Register(savePicker);
        savePicker.FileTypeChoices.Add(new("JSON", new List<string>() { ".json" }));
        savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

        windowHandler.Register(filePicker);
        filePicker.FileTypeFilter.Add(".json");

        IsUpdateVisible = Updater.IsUpdateAvailable;

        UseCustomTitleBar = Configuration.Developer.UseCustomTitleBar;
        UseSystemTheme = Configuration.Apperance.UseSystemTheme;
        IsVibrancyEnabled = Computer.IsWindows11 && !Configuration.Apperance.UseSystemTheme;

    }
    #endregion


    #region About
    [RelayCommand]
    async Task ShowDialogAsync()
    {
        ContentDialog dialog = UIElementProvider.AboutDialog(Updater.CurrentAppVersion, Updater.CurrentApiVersion, RequestFeedbackCommand);

        await message.ShowAsync(dialog);
    }

    [RelayCommand]
    async Task RequestFeedbackAsync()
    {
        Feedback result = await feedbackRequest.ShowAsync(true);

        if (result.Result == FeedbackResult.Submit)
            await feedbackRequest.SubmitAsync(result);
    }
    #endregion


    #region Debug
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
    bool useCustomTitleBar;

    partial void OnUseCustomTitleBarChanged(
        bool value)
    {
        if (value != windowHandler.HasCustomTitleBar)
            windowHandler.SetTitleBar(value ? uIElementReciever.TitleBarDragArea : null, uIElementReciever.TitleBarContainer);

        Configuration.Developer.UseCustomTitleBar = value;

    }
    #endregion


    #region Update
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
    #endregion


    #region Directories
    readonly FolderPicker folderPicker = new() { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };

    [RelayCommand]
    async Task SelectDirectoryAsync(
        bool texturepackDirectory)
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
            await message.ShowAsync("Something went wrong :(", "It looks like IcyLauncher cant write to this directory or it does not exist. Please verify that you have given permissions to IcyLauncher and this directory still exists.");
    }

    [RelayCommand]
    void ResetDirectory(
        bool texturepackDirectory)
    {
        if (texturepackDirectory)
            Configuration.Launcher.TexturepackDirectory = $"{Computer.MinecraftDirectory}\\games\\com.mojang\\resource_packs";
        else
            Configuration.Launcher.VersionsDirectory = $"{Computer.CurrentDirectory}\\Versions";
    }
    #endregion


    #region Navigation
    [RelayCommand]
    void NavigateTo(
        string page) =>
        navigation.SetCurrentPage(page.AsType());
    #endregion


    #region Theme
    [RelayCommand]
    async Task ResetColorsAsync(
        bool darkMode)
    {
        if (await message.ShowAsync("Are you sure?", $"If you click Ok your current color settings will be overwritten by the default {(darkMode ? "dark" : "light")} mode colors.\nThis will not effect your current accent colors.\nThis will also effect the blur color mode.", closeButton: "Cancel", primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        themeManager.Load(darkMode ? Theme.Dark : Theme.Light, true);
        Configuration.Apperance.IsDarkModeBackdropEnabled = darkMode;
    }


    public static bool IsDarkModeBackdropEnabled(
        int selectedIndex) =>
        selectedIndex == 0 || selectedIndex == 1;


    [ObservableProperty]
    bool useSystemTheme;

    partial void OnUseSystemThemeChanged(
        bool value)
    {
        if (Configuration.Apperance.Backdrop == Backdrop.Vibrancy)
            Configuration.Apperance.Backdrop = Backdrop.None;

        Configuration.Apperance.UseSystemTheme = value;

        IsVibrancyEnabled = Computer.IsWindows11 && !Configuration.Apperance.UseSystemTheme;
    }


    [ObservableProperty]
    bool isVibrancyEnabled;
    #endregion


    #region Configuration
    readonly FileSavePicker savePicker = new() { SuggestedStartLocation = PickerLocationId.Desktop };

    [RelayCommand]
    async Task ExportAsync(
        bool saveConfig)
    {
        savePicker.SuggestedFileName = saveConfig ? "Config" : "Theme";
        StorageFile save = await savePicker.PickSaveFileAsync();

        if (save is null || string.IsNullOrWhiteSpace(save.Path))
            return;

        if (!fileSystem.FileWritable(save.Path))
        {
            await message.ShowAsync("Something went wrong :(", "It looks like IcyLauncher cant write to this file. Please verify that you have given permissions to IcyLauncher.");
            return;
        }

        await fileSystem.SaveAsTextAsync(save.Path, saveConfig ? configurationManager.Export() : themeManager.Export(), true).ConfigureAwait(false);
    }

    readonly FileOpenPicker filePicker = new() { SuggestedStartLocation = PickerLocationId.Desktop };

    [RelayCommand]
    async Task LoadAsync(
        bool loadConfig)
    {
        StorageFile file = await filePicker.PickSingleFileAsync();

        if (file is null || string.IsNullOrWhiteSpace(file.Path))
            return;

        if (!fileSystem.FileExists(file.Path))
        {
            await message.ShowAsync("Something went wrong :(", "It looks like this file does no longer exist. Please verify the file still exists.", true);
            return;
        }

        string str = loadConfig ? "configuration" : "theme";
        if (await message.ShowAsync("Are you sure?", $"Do you really want to overwrite your current {str} by this external {str}?\nLoading external {str}s can be dangerous. Make sure you backup your current {str}.\nDo you want to continue?", closeButton: "No", primaryButton: "Yes") != ContentDialogResult.Primary)
            return;

        if (loadConfig)
        {
            if (!converter.TryToObject(out Configuration? configuration, await fileSystem.ReadAsTextAsync(file.Path)))
            {
                await message.ShowAsync("Something went wrong :(", "It looks like this is not a valid configuration.\nPlease verify the file is a proper JSON and every property is being set.");
                return;
            }

            configurationManager.Load(configuration!, true);
            return;
        }
        else
        {
            if (!converter.TryToObject(out Theme? configuration, await fileSystem.ReadAsTextAsync(file.Path)))
            {
                await message.ShowAsync("Something went wrong :(", "It looks like this is not a valid theme.\nPlease verify the file is a proper JSON and every property is being set.");
                return;
            }

            themeManager.Load(configuration!);
        }
    }
    #endregion
}