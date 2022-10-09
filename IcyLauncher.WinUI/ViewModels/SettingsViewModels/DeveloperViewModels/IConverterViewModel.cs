using Microsoft.UI;
using Newtonsoft.Json;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class IConverterViewModel : ObservableObject
{
    readonly IConverter converter;
    readonly IMessage message;

    public IConverterViewModel(
        IConverter converter,
        IMessage message)
    {
        this.converter = converter;
        this.message = message;
    }


    [ObservableProperty]
    SolidColor object_ = new(Colors.Red, "Red");

    [ObservableProperty]
    string input = "";


    [ObservableProperty]
    Formatting formatting = Formatting.None;

    [RelayCommand]
    async Task ToStringAsync()
    {
        try
        {
            input = converter.ToString(Object_, Formatting);
            await message.ShowAsync("converter.ToString()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("converter.ToString()", $"Method completed.\nException{ex.Format()}");
        }
    }

    [RelayCommand]
    async Task ToObjectAsync()
    {
        try
        {
            Object_ = converter.ToObject<SolidColor>(Input);
            await message.ShowAsync("converter.ToObject<T>()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("converter.ToObject<T>()", $"Method completed.\nException{ex.Format()}");
        }
    }

    [RelayCommand]
    async Task TryToObjectAsync()
    {
        try
        {
            bool result = converter.TryToObject(out SolidColor? _, Input);
            await message.ShowAsync("converter.TryToObject<T>()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("converter.TryToObject<T>()", $"Method completed.\nException{ex.Format()}");
        }
    }
}