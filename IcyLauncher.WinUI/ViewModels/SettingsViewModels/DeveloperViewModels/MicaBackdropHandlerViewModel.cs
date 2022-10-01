namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    void SetupMicaBackdropHandlerViewModel() =>
        MicaBackdropHandler_isDarkModeEnabled = micaBackdropHandler.IsDarkModeEnabled;


    [RelayCommand]
    async Task MicaBackdropHandler_EnableBackdrop()
    {
        try
        {
            bool result = micaBackdropHandler.EnableBackdrop();
            await message.ShowAsync("micaBackdropHandler.EnableBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("micaBackdropHandler.EnableBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task MicaBackdropHandler_DisableBackdrop()
    {
        try
        {
            bool result = micaBackdropHandler.DisableBackdrop();
            await message.ShowAsync("micaBackdropHandler.DisableBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("micaBackdropHandler.DisableBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    bool micaBackdropHandler_isDarkModeEnabled = default!;

    async partial void OnMicaBackdropHandler_isDarkModeEnabledChanged(bool value)
    {
        try
        {
            if (micaBackdropHandler.IsDarkModeEnabled != value)
            {
                micaBackdropHandler.IsDarkModeEnabled = value;
                await message.ShowAsync("micaBackdropHandler.IsDarkModeEnabled", $"Method completed.", closeButton: "Ok");
            }
        }
        catch (Exception ex)
        {
            await message.ShowAsync("micaBackdropHandler.IsDarkModeEnabled", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}