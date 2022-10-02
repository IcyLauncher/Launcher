using Microsoft.UI;
using Microsoft.UI.Composition;
using System.Numerics;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class ImagingUtilityViewModel : ObservableObject
{
    readonly ImagingUtility imagingUtility;
    readonly IMessage message;

    public ImagingUtilityViewModel(
        ImagingUtility imagingUtility,
        IMessage message)
    {
        this.imagingUtility = imagingUtility;
        this.message = message;
    }


    [ObservableProperty]
    string compositorText = "[Compositor: null]\n";

    [ObservableProperty]
    string containerVisualText = "[ContainerVisual: null]\n";

    [ObservableProperty]
    string spriteVisualText = "[SpriteVisual: null]\n";

    [ObservableProperty]
    string spriteVisualBrushText = "[SpriteVisual.Brush: null]";


    Compositor? compositor = null;
    ContainerVisual? container = null;
    SpriteVisual? spriteVisual = null;
    CompositionBrush? _spriteVisualBrush = null;
    CompositionBrush? spriteVisualBrush
    {
        get => _spriteVisualBrush;
        set
        {
            _spriteVisualBrush = value;

            if (spriteVisual is not null)
                spriteVisual.Brush = value;
        }
    }


    [RelayCommand]
    async Task InitializeUIElementAsync(
        UIElement element)
    {
        try
        {
            imagingUtility.InitializeUIElement(element, out compositor, out container);

            bool result1 = compositor is not null;
            bool result2 = container is not null;
            compositorText = $"[Compositor: {(result1 ? compositor!.GetHashCode() : "null")}]\n";
            containerVisualText = $"[ContainerVisual: {(result2 ? $"(Rectangle){container!.GetHashCode()}" : "null")}]\n";

            await message.ShowAsync("imagingUtility.InitializeUIElement()", $"Method completed.\nResult: [Compositor: {result1}], Container: [{result2}]", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.InitializeUIElement()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int startPointX = 0;
    [ObservableProperty]
    int startPointY = 0;

    [ObservableProperty]
    int endPointX = 1;
    [ObservableProperty]
    int endPointY = 1;

    [RelayCommand]
    async Task CreateGradientBrushAsync()
    {
        try
        {
            spriteVisualBrush = imagingUtility.CreateGradientBrush(
                compositor!,
                new Vector2(StartPointX, StartPointY),
                new Vector2(EndPointX, EndPointY),
                new[] { (0.0f, Colors.Red), (0.5f, Colors.Green), (1.0f, Colors.Blue) });

            bool result = spriteVisualBrush is not null;
            spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionLinearGradientBrush){spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateGradientBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateGradientBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    Color maskColor = Colors.MediumPurple;

    [RelayCommand]
    async Task CreateMaskBrushAsync()
    {
        try
        {
            spriteVisualBrush = imagingUtility.CreateMaskBrush(
                compositor!,
                imagingUtility.CreateColorBrush(
                    compositor!,
                    MaskColor),
                imagingUtility.CreateGradientBrush(
                    compositor!,
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new[] { (0.0f, Colors.White), (1.0f, Colors.Transparent) }));

            bool result = spriteVisualBrush is not null;
            spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionMaskBrush){spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateMaskBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateMaskBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    CompositionStretch stretch = CompositionStretch.Fill;

    [ObservableProperty]
    float horizontalAlignment = 0.0f;

    [ObservableProperty]
    float verticalAlignment = 0.5f;

    [RelayCommand]
    async Task CreateImageBrushAsync(
        string uriSource)
    {
        try
        {
            spriteVisualBrush = imagingUtility.CreateImageBrush(
                compositor!,
                new Uri(uriSource),
                Stretch,
                HorizontalAlignment, VerticalAlignment);

            bool result = spriteVisualBrush is not null;
            spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionSurfaceBrush){spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateImageBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateImageBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    Color color = Colors.MediumPurple;

    [RelayCommand]
    async Task CreateColorBrushAsync()
    {
        try
        {
            spriteVisualBrush = imagingUtility.CreateColorBrush(
                compositor!,
                Color);

            bool result = spriteVisualBrush is not null;
            spriteVisualBrushText = $"[SpriteVisual.Brush: {(result ? $"(CompositionColorBrush){spriteVisualBrush!.GetHashCode()}" : "null")}]";

            await message.ShowAsync("imagingUtility.CreateColorBrush()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateColorBrush()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int sizeWidth = 400;
    [ObservableProperty]
    int sizeHeight = 200;

    [ObservableProperty]
    int offsetX = 0;
    [ObservableProperty]
    int offsetY = 0;
    [ObservableProperty]
    int offsetZ = 0;

    [RelayCommand]
    async Task CreateSpriteVisualAsync()
    {
        try
        {
            spriteVisual = imagingUtility.CreateSpriteVisual(
                compositor!,
                new Vector2(SizeWidth, SizeHeight),
                spriteVisualBrush,
                new Vector3(OffsetX, OffsetY, OffsetZ));

            container!.Children.RemoveAll();
            container!.Children.InsertAtTop(spriteVisual);

            bool result = spriteVisual is not null;
            spriteVisualText = $"[SpriteVisual: {(result ? spriteVisual!.GetHashCode() : "null")}]\n";

            await message.ShowAsync("imagingUtility.CreateSpriteVisual()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("imagingUtility.CreateSpriteVisual()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}