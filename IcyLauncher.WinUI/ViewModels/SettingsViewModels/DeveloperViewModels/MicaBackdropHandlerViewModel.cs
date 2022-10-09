namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class MicaBackdropHandlerViewModel : ObservableObject
{
    readonly IBackdropHandler micaBackdropHandler;
    readonly IMessage message;

    public MicaBackdropHandlerViewModel(
        IBackdropHandler micaBackdropHandler,
        IMessage message)
    {
        this.micaBackdropHandler = micaBackdropHandler;
        this.message = message;


        IsDarkModeEnabled = micaBackdropHandler.IsDarkModeEnabled;
    }


    [RelayCommand]
    async Task EnableBackdropAsync()
    {
        try
        {
            bool result = micaBackdropHandler.EnableBackdrop();
            await message.ShowAsync("micaBackdropHandler.EnableBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("micaBackdropHandler.EnableBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }

    [RelayCommand]
    async Task DisableBackdropAsync()
    {
        try
        {
            bool result = micaBackdropHandler.DisableBackdrop();
            await message.ShowAsync("micaBackdropHandler.DisableBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("micaBackdropHandler.DisableBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }


    [ObservableProperty]
    bool isDarkModeEnabled = default!;

    async partial void OnIsDarkModeEnabledChanged(bool value)
    {
        try
        {
            if (micaBackdropHandler.IsDarkModeEnabled != value)
            {
                micaBackdropHandler.IsDarkModeEnabled = value;
                await message.ShowAsync("micaBackdropHandler.IsDarkModeEnabled", $"Method completed.");
            }
        }
        catch (Exception ex)
        {
            await message.ShowAsync("micaBackdropHandler.IsDarkModeEnabled", $"Method completed.\nException{ex.Format()}");
        }
    }
}