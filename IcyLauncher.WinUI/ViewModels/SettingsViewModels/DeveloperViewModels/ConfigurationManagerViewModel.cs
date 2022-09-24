namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    string configurationManager_currentConfig = "";

    [ObservableProperty]
    bool configurationManager_ignoreTheme = false;

    [RelayCommand]
    void ConfigurationManager_Export() =>
         ConfigurationManager_currentConfig = configurationManager.Export();

    [RelayCommand]
    async Task ConfigurationManager_Load()
    {
        if (converter.TryToObject(out Configuration? configuration, ConfigurationManager_currentConfig) == true)
            configurationManager.Load(configuration!, ConfigurationManager_ignoreTheme);
        else
            await message.ShowAsync("Something went wrong :(", "It looks like this is not a valid configuration. Please verify every property is being set.");
    }
}