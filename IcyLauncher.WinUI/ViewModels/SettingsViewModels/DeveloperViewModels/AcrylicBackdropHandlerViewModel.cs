namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class AcrylicBackdropHandlerViewModel : ObservableObject
{
    #region Setup
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
    #endregion


    #region EnableBackdrop
    [RelayCommand]
    async Task EnableBackdropAsync()
    {
        try
        {
            bool result = acrylicBackdropHandler.EnableBackdrop();
            await message.ShowAsync("acrylicBackdropHandler.EnableBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("acrylicBackdropHandler.EnableBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region DisableBackdrop
    [RelayCommand]
    async Task DisableBackdropAsync()
    {
        try
        {
            bool result = acrylicBackdropHandler.DisableBackdrop();
            await message.ShowAsync("acrylicBackdropHandler.DisableBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("acrylicBackdropHandler.DisableBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region IsDarkModeEnabled
    [ObservableProperty]
    bool isDarkModeEnabled = default!;

    async partial void OnIsDarkModeEnabledChanged(
        bool value)
    {
        try
        {
            if (acrylicBackdropHandler.IsDarkModeEnabled != value)
            {
                acrylicBackdropHandler.IsDarkModeEnabled = value;
                await message.ShowAsync("acrylicBackdropHandler.IsDarkModeEnabled", $"Method completed.");
            }
        }
        catch (Exception ex)
        {
            await message.ShowAsync("acrylicBackdropHandler.IsDarkModeEnabled", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}