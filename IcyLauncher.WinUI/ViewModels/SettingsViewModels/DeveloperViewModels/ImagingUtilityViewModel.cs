using Microsoft.UI;
using Microsoft.UI.Composition;
using System.Numerics;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    string imagingUtility_compositorText = "[Compositor: null]\n";

    [ObservableProperty]
    string imagingUtility_containerVisualText = "[ContainerVisual: null]\n";

    [ObservableProperty]
    string imagingUtility_spriteVisualText = "[SpriteVisual: null]\n";

    [ObservableProperty]
    string imagingUtility_spriteVisualBrushText = "[SpriteVisual.Brush: null]";


    Compositor? imagingUtility_compositor = null;
    ContainerVisual? imagingUtility_container = null;
    SpriteVisual? imagingUtility_spriteVisual = null;
    CompositionBrush? _imagingUtility_spriteVisualBrush = null;
    CompositionBrush? imagingUtility_spriteVisualBrush
    {
        get => _imagingUtility_spriteVisualBrush;
        set
        {
            _imagingUtility_spriteVisualBrush = value;

            if (imagingUtility_spriteVisual is not null)
                imagingUtility_spriteVisual.Brush = value;
        }
    }


    [RelayCommand]
    async Task ImagingUtility_InitializeUIElement(UIElement element)
    {
        try
        {
            imagingUtility.InitializeUIElement(element, out imagingUtility_compositor, out imagingUtility_container);

            bool result1 = imagingUtility_compositor is not null;
            bool result2 = imagingUtility_container is not null;
            ImagingUtility_compositorText = $"[Compositor: {(result1 ? imagingUtility_compositor!.GetHashCode() : "null")}]\n";
            ImagingUtility_containerVisualText = $"[ContainerVisual: {(result2 ? $"(Rectangle){imagingUtility_container!.GetHashCode()}" : "null")}]\n";

            await message.ShowAsync("imagingUtility.InitializeUIElement()", $"Method completed.\nResult: [Compositor: {result1}], Container: [{result2}]", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.InitializeUIElement()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int imagingUtility_startPointX = 0;
    [ObservableProperty]
    int imagingUtility_startPointY = 0;

    [ObservableProperty]
    int imagingUtility_endPointX = 1;
    [ObservableProperty]
    int imagingUtility_endPointY = 1;

    [RelayCommand]
    async Task ImagingUtility_CreateGradientBrush()
    {
        try
        {
            imagingUtility_spriteVisualBrush = imagingUtility.CreateGradientBrush(
                imagingUtility_compositor!,
                new Vector2(ImagingUtility_startPointX, ImagingUtility_startPointY),
                new Vector2(ImagingUtility_endPointX, ImagingUtility_endPointY),
                new[] { (0.0f, Colors.Red), (0.5f, Colors.Green), (1.0f, Colors.Blue) });

            bool result = imagingUtility_spriteVisualBrush is not null;
            ImagingUtility_spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionLinearGradientBrush){imagingUtility_spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateGradientBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateGradientBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    Color imagingUtility_maskColor = Colors.MediumPurple;

    [RelayCommand]
    async Task ImagingUtility_CreateMaskBrush()
    {
        try
        {
            imagingUtility_spriteVisualBrush = imagingUtility.CreateMaskBrush(
                imagingUtility_compositor!,
                imagingUtility.CreateColorBrush(
                    imagingUtility_compositor!,
                    ImagingUtility_maskColor),
                imagingUtility.CreateGradientBrush(
                    imagingUtility_compositor!,
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new[] { (0.0f, Colors.White), (1.0f, Colors.Transparent) }));

            bool result = imagingUtility_spriteVisualBrush is not null;
            ImagingUtility_spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionMaskBrush){imagingUtility_spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateMaskBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateMaskBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    CompositionStretch imagingUtility_stretch = CompositionStretch.Fill;

    [ObservableProperty]
    float imagingUtility_horizontalAlignment = 0.0f;

    [ObservableProperty]
    float imagingUtility_verticalAlignment = 0.5f;

    [RelayCommand]
    async Task ImagingUtility_CreateImageBrush(string uriSource)
    {
        try
        {
            imagingUtility_spriteVisualBrush = imagingUtility.CreateImageBrush(
                imagingUtility_compositor!,
                new Uri(uriSource),
                ImagingUtility_stretch,
                ImagingUtility_horizontalAlignment, ImagingUtility_verticalAlignment);

            bool result = imagingUtility_spriteVisualBrush is not null;
            ImagingUtility_spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionSurfaceBrush){imagingUtility_spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateImageBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateImageBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    Color imagingUtility_color = Colors.MediumPurple;

    [RelayCommand]
    async Task ImagingUtility_CreateColorBrush()
    {
        try
        {
            imagingUtility_spriteVisualBrush = imagingUtility.CreateColorBrush(
                imagingUtility_compositor!,
                ImagingUtility_color);

            bool result = imagingUtility_spriteVisualBrush is not null;
            ImagingUtility_spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionColorBrush){imagingUtility_spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateColorBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateColorBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int imagingUtility_sizeWidth = 400;
    [ObservableProperty]
    int imagingUtility_sizeHeight = 200;

    [ObservableProperty]
    int imagingUtility_offsetX = 0;
    [ObservableProperty]
    int imagingUtility_offsetY = 0;
    [ObservableProperty]
    int imagingUtility_offsetZ = 0;

    [RelayCommand]
    async Task ImagingUtility_CreateSpriteVisual()
    {
        try
        {
            imagingUtility_spriteVisual = imagingUtility.CreateSpriteVisual(
                imagingUtility_compositor!,
                new Vector2(ImagingUtility_sizeWidth, ImagingUtility_sizeHeight),
                imagingUtility_spriteVisualBrush,
                new Vector3(ImagingUtility_offsetX, ImagingUtility_offsetY, ImagingUtility_offsetZ));

            imagingUtility_container!.Children.RemoveAll();
            imagingUtility_container!.Children.InsertAtTop(imagingUtility_spriteVisual);

            bool result = imagingUtility_spriteVisual is not null;
            imagingUtility_spriteVisualText = $"[SpriteVisual: {(result ? imagingUtility_spriteVisual!.GetHashCode() : "null")}]\n";

            await message.ShowAsync("imagingUtility.CreateSpriteVisual()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateSpriteVisual()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

}