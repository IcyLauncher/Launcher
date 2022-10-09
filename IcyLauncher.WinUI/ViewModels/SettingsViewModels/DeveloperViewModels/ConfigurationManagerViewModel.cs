namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class ConfigurationManagerViewModel : ObservableObject
{
    readonly ConfigurationManager configurationManager;
    readonly IConverter converter;
    readonly IMessage message;

    public ConfigurationManagerViewModel(
        ConfigurationManager configurationManager,
        IConverter converter,
        IMessage message)
    {
        this.configurationManager = configurationManager;
        this.converter = converter;
        this.message = message;
    }


    [ObservableProperty]
    string currentConfig = "";

    [ObservableProperty]
    bool ignoreTheme = false;

    [RelayCommand]
    async Task ExportAsync()
    {
        try
        {
            CurrentConfig = configurationManager.Export();
            await message.ShowAsync("configurationManager.Export()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("configurationManager.Export()", $"Method completed.\nException{ex.Format()}");
        }
    }

    [RelayCommand]
    async Task LoadAsync()
    {
        try
        {
            Configuration configuration = converter.ToObject<Configuration>(CurrentConfig);
            configurationManager.Load(configuration!, IgnoreTheme);

            await message.ShowAsync("configurationManager.Load()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("configurationManager.Load()", $"Method completed.\nException{ex.Format()}");
        }
    }
}