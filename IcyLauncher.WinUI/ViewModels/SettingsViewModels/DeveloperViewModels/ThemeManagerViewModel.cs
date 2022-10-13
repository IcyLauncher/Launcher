using Microsoft.UI;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class ThemeManagerViewModel : ObservableObject
{
    #region Setup
    readonly ThemeManager themeManager;
    readonly IMessage message;

    public ThemeManagerViewModel(
        ThemeManager themeManager,
        IMessage message)
    {
        this.themeManager = themeManager;
        this.message = message;
    }
    #endregion


    #region IsSubscribedToUISettings
    [ObservableProperty]
    bool isSubscribedToUISettings;

    [RelayCommand]
    void UpdateIsSubscribedToUISettings() =>
        IsSubscribedToUISettings = themeManager.IsSubscribedToUISettings;
    #endregion


    #region SubscribeToUISettings
    [RelayCommand]
    async Task SubscribeToUISettingsAsync(
        bool subscribe)
    {
        try
        {
            themeManager.SubscribeToUISettings(subscribe);
            await message.ShowAsync("themeManager.SubscribeToUISettings()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.SubscribeToUISettings()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region ModifyColor
    [ObservableProperty]
    double percentage = 50;

    [ObservableProperty]
    Color sourceColor = Colors.Blue;

    [RelayCommand]
    async Task ModifyColorAsync()
    {
        try
        {
            SourceColor = ThemeManager.ModifyColor(SourceColor, Percentage / 100);
            await message.ShowAsync("ThemeManager.ModifyColor()", $"Method completed.\nResult: {SourceColor}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("ThemeManager.ModifyColor()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region RandomizeTheme
    [RelayCommand]
    async Task RandomizeThemeAsync()
    {
        try
        {
            themeManager.RandomizeTheme();
            await message.ShowAsync("themeManager.RandomizeTheme()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.RandomizeTheme()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region SetResourceColors
    [RelayCommand]
    async Task SetResourceColorsAsync()
    {
        try
        {
            themeManager.SetResourceColors();
            await message.ShowAsync("themeManager.SetResourceColors()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.SetResourceColors()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region SetUnbindableBindings
    [RelayCommand]
    async Task SetUnbindableBindingsAsync()
    {
        try
        {
            themeManager.SetUnbindableBindings();
            await message.ShowAsync("themeManager.SetUnbindableBindings()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.SetUnbindableBindings()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region GetRandomColor
    [ObservableProperty]
    byte transparency = 255;

    [ObservableProperty]
    Color randomColor = Colors.Black;

    [RelayCommand]
    async Task GetRandomColorAsync()
    {
        try
        {
            RandomColor = themeManager.GetRandomColor(Transparency);
            await message.ShowAsync("themeManager.GetRandomColor()", $"Method completed.\nResult: {randomColor}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.GetRandomColor()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}