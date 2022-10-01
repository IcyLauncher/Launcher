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
    void IConverter_ToString() =>
        IConverter_input = converter.ToString(IConverter_object, IConverter_formatting);

    [RelayCommand]
    async void IConverter_ToObject()
    {
        if (converter.TryToObject(out SolidColor? solidColor, IConverter_input) == true)
            IConverter_object = solidColor!;
        else
            await message.ShowAsync("Something went wrong :(", "It looks like this is not a valid SolidColor.\nPlease verify the input is a proper JSON and every property is being set.", closeButton: "Ok");
    }

    [RelayCommand]
    async void IConverter_TryToObject() =>
            await message.ShowAsync("TryToObject - Result", $"Method testing returned: {converter.TryToObject(out SolidColor? _, IConverter_input)}", closeButton: "Ok");
}