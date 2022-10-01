using Microsoft.UI;
using Newtonsoft.Json;

namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    SolidColor iConverter_object = new(Colors.Red, "Red");

    [ObservableProperty]
    string iConverter_input = "";


    [ObservableProperty]
    Formatting iConverter_formatting = Formatting.None;

    [RelayCommand]
    async Task IConverter_ToString()
    {
        try
        {
            IConverter_input = converter.ToString(IConverter_object, IConverter_formatting);
            await message.ShowAsync("converter.ToString()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("converter.ToString()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task IConverter_ToObject()
    {
        try
        {
            IConverter_object = converter.ToObject<SolidColor>(IConverter_input);
            await message.ShowAsync("converter.ToObject<T>()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("converter.ToObject<T>()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task IConverter_TryToObject()
    {
        try
        {
            bool result = converter.TryToObject(out SolidColor? _, IConverter_input);
            await message.ShowAsync("converter.TryToObject<T>()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("converter.TryToObject<T>()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}