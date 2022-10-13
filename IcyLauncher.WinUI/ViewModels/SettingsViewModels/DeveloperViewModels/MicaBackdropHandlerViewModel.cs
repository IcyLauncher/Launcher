namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class MicaBackdropHandlerViewModel : ObservableObject
{
    #region Setup
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
    #endregion


    #region EnableBackdrop
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
    #endregion

    #region DisableBackdrop
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
    #endregion


    #region IsDarkModeenabled
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
    #endregion
}