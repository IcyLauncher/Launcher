namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class AcrylicBackdropHandlerViewModel : ObservableObject
{
    readonly IBackdropHandler acrylicBackdropHandler;
    readonly IMessage message;

    public AcrylicBackdropHandlerViewModel(
        IBackdropHandler acrylicBackdropHandler,
        IMessage message)
    {
        this.acrylicBackdropHandler = acrylicBackdropHandler;
        this.message = message;


        IsDarkModeEnabled = acrylicBackdropHandler.IsDarkModeEnabled;
    }


    [RelayCommand]
    async Task EnableBackdropAsync()
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
    async Task DisableBackdropAsync()
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
    bool isDarkModeEnabled = default!;

    async partial void OnIsDarkModeEnabledChanged(bool value)
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