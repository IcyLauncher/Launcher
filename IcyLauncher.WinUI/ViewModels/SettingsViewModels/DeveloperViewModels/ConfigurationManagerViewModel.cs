namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    string configurationManager_currentConfig = "";

    [ObservableProperty]
    bool configurationManager_ignoreTheme = false;

    [RelayCommand]
    async Task ConfigurationManager_Export()
    {
        try
        {
            ConfigurationManager_currentConfig = configurationManager.Export();
            await message.ShowAsync("configurationManager.Export()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("configurationManager.Export()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task ConfigurationManager_Load()
    {
        try
        {
            Configuration configuration = converter.ToObject<Configuration>(ConfigurationManager_currentConfig);
            configurationManager.Load(configuration!, ConfigurationManager_ignoreTheme);

            await message.ShowAsync("configurationManager.Load()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("configurationManager.Load()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}