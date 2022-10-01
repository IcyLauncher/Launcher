using Microsoft.UI;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [RelayCommand]
    async Task ThemeManager_RandomizeTheme()
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
    async Task ThemeManager_SetResourceColors()
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
    async Task ThemeManager_SetUnbindableBindings()
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
    byte themeManager_transparency = 255;

    [ObservableProperty]
    Color themeManager_randomColor = Colors.Black;

    [RelayCommand]
    async Task ThemeManager_GetRandomColor()
    {
        try
        {
            ThemeManager_randomColor = themeManager.GetRandomColor(ThemeManager_transparency);
            await message.ShowAsync("themeManager.GetRandomColor()", $"Method completed.\nResult: {ThemeManager_randomColor}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("themeManager.GetRandomColor()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}