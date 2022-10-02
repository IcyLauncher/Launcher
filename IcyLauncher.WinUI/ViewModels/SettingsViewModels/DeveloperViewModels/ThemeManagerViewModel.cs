using Microsoft.UI;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class ThemeManagerViewModel : ObservableObject
{
    readonly ThemeManager themeManager;
    readonly IMessage message;

    public ThemeManagerViewModel(
        ThemeManager themeManager,
        IMessage message)
    {
        this.themeManager = themeManager;
        this.message = message;
    }


    [RelayCommand]
    async Task RandomizeThemeAsync()
    {
        try
        {
            themeManager.RandomizeTheme();
            await message.ShowAsync("themeManager.RandomizeTheme()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.RandomizeTheme()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task SetResourceColorsAsync()
    {
        try
        {
            themeManager.SetResourceColors();
            await message.ShowAsync("themeManager.SetResourceColors()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.SetResourceColors()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task SetUnbindableBindingsAsync()
    {
        try
        {
            themeManager.SetUnbindableBindings();
            await message.ShowAsync("themeManager.SetUnbindableBindings()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.SetUnbindableBindings()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


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
            await message.ShowAsync("themeManager.GetRandomColor()", $"Method completed.\nResult: {randomColor}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.GetRandomColor()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}