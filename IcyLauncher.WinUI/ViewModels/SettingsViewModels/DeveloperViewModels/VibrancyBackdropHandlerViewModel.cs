namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class VibrancyBackdropHandlerViewModel : ObservableObject
{
    #region Setup
    readonly IBackdropHandler vibrancyBackdropHandler;
    readonly IMessage message;

    public VibrancyBackdropHandlerViewModel(
        IBackdropHandler vibrancyBackdropHandler,
        IMessage message)
    {
        this.vibrancyBackdropHandler = vibrancyBackdropHandler;
        this.message = message;
    }
    #endregion


    #region EnableBackdrop
    [RelayCommand]
    async Task EnableBackdropAsync()
    {
        try
        {
            bool result = vibrancyBackdropHandler.EnableBackdrop();
            await message.ShowAsync("vibrancyBackdropHandler.EnableBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("vibrancyBackdropHandler.EnableBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region DisableBackdrop
    [RelayCommand]
    async Task DisableBackdropAsync()
    {
        try
        {
            bool result = vibrancyBackdropHandler.DisableBackdrop();
            await message.ShowAsync("vibrancyBackdropHandler.DisableBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("vibrancyBackdropHandler.DisableBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}