namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class BackdropHandlerViewModel : ObservableObject
{
    #region Setup
    readonly BackdropHandler backdropHandler;
    readonly IMessage message;

    public BackdropHandlerViewModel(
        BackdropHandler backdropHandler,
        IMessage message)
    {
        this.backdropHandler = backdropHandler;
        this.message = message;


        UpdateIsMicaEnabled();
        UpdateIsAcrylicEnabled();
        UpdateIsVibrancyEnabled();
        UpdateIsNoneEnabled();
        UpdateCurrent();
    }
    #endregion


    #region IsMicaEnabled
    [ObservableProperty]
    bool isMicaEnabled = default!;

    [RelayCommand]
    void UpdateIsMicaEnabled() =>
        IsMicaEnabled = backdropHandler.IsMicaEnabled;
    #endregion

    #region IsAcrylicEnabled
    [ObservableProperty]
    bool isAcrylicEnabled = default!;

    [RelayCommand]
    void UpdateIsAcrylicEnabled() =>
        IsAcrylicEnabled = backdropHandler.IsAcrylicEnabled;
    #endregion

    #region IsVibrancyEnabled
    [ObservableProperty]
    bool isVibrancyEnabled = default!;

    [RelayCommand]
    void UpdateIsVibrancyEnabled() =>
        IsVibrancyEnabled = backdropHandler.IsVibrancyEnabled;
    #endregion

    #region IsNoneEnabled
    [ObservableProperty]
    bool isNoneEnabled = default!;

    [RelayCommand]
    void UpdateIsNoneEnabled() =>
        IsNoneEnabled = backdropHandler.IsNoneEnabled;
    #endregion

    #region Current
    [ObservableProperty]
    Backdrop? current = default!;

    [RelayCommand]
    void UpdateCurrent() =>
        Current = backdropHandler.Current;
    #endregion


    #region SetBackdrop
    [ObservableProperty]
    Backdrop backdrop;
    
    [ObservableProperty]
    bool enable = true;
    
    [ObservableProperty]
    bool useDarkMode = true;
    
    [ObservableProperty]
    bool useDarkModeIsNull;

    [RelayCommand]
    async Task SetBackdropAsync()
    {
        try
        {
            bool result = backdropHandler.SetBackdrop(Backdrop, Enable, UseDarkModeIsNull ? null : UseDarkMode);
            await message.ShowAsync("backdropHandler.SetBackdrop()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("backdropHandler.SetBackdrop()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region SetDarkMode
    [RelayCommand]
    async Task SetDarkModeAsync()
    {
        try
        {
            backdropHandler.SetDarkMode(Backdrop, UseDarkMode);
            await message.ShowAsync("backdropHandler.SetDarkMode()", $"Method completed");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("backdropHandler.SetDarkMode()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}