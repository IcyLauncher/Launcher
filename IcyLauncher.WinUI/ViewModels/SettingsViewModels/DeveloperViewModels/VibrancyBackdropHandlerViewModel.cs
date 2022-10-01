namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [RelayCommand]
    async Task VibrancyBackdropHandler_EnableBackdrop()
    {
        try
        {
            bool result = vibrancyBackdropHandler.EnableBackdrop();
            await message.ShowAsync("vibrancyBackdropHandler.EnableBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("vibrancyBackdropHandler.EnableBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task VibrancyBackdropHandler_DisableBackdrop()
    {
        try
        {
            bool result = vibrancyBackdropHandler.DisableBackdrop();
            await message.ShowAsync("vibrancyBackdropHandler.DisableBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("vibrancyBackdropHandler.DisableBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}