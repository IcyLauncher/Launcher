namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    void SetupAcrylicBackdropHandlerViewModel() =>
        AcrylicBackdropHandler_isDarkModeEnabled = acrylicBackdropHandler.IsDarkModeEnabled;


    [RelayCommand]
    async Task AcrylicBackdropHandler_EnableBackdrop()
    {
        try
        {
            bool result = acrylicBackdropHandler.EnableBackdrop();
            await message.ShowAsync("acrylicBackdropHandler.EnableBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("acrylicBackdropHandler.EnableBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task AcrylicBackdropHandler_DisableBackdrop()
    {
        try
        {
            bool result = acrylicBackdropHandler.DisableBackdrop();
            await message.ShowAsync("acrylicBackdropHandler.DisableBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("acrylicBackdropHandler.DisableBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    bool acrylicBackdropHandler_isDarkModeEnabled = default!;

    async partial void OnAcrylicBackdropHandler_isDarkModeEnabledChanged(bool value)
    {
        try
        {
            if (acrylicBackdropHandler.IsDarkModeEnabled != value)
            {
                acrylicBackdropHandler.IsDarkModeEnabled = value;
                await message.ShowAsync("acrylicBackdropHandler.IsDarkModeEnabled", $"Method completed.", closeButton: "Ok");
            }
        }
        catch (Exception ex)
        {
            await message.ShowAsync("acrylicBackdropHandler.IsDarkModeEnabled", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}