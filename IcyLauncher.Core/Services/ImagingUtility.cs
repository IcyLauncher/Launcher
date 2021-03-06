using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using System.Numerics;
using Windows.UI;

namespace IcyLauncher.Core.Services;

public class ImagingUtility
{
    readonly ILogger<ImagingUtility> logger;

    public ImagingUtility(ILogger<ImagingUtility> logger)
    {
        this.logger = logger;

        this.logger.Log("Registered ImagingUtility");
    }


    public void InitializeUIElement(UIElement element, out Compositor? compositor, out ContainerVisual? container)
    {

        try
        {
            container = ElementCompositionPreview.GetElementVisual(element).Compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(element, container);
            compositor = container.Compositor;

            logger.Log("Initialized UIElement compositor and containerVisual");
        }
        catch (Exception ex)
        {
            container = null;
            compositor = null;

            logger.Log("Failed to initialize UIElement compositor and containerVisual", ex);
        }
    }

    public CompositionLinearGradientBrush? CreateGradientBrush(Compositor compositor, Vector2 startPoint, Vector2 endPoint, (float offset, Color color)[] gradientStops)
    {
        try
        {
            var brush = compositor.CreateLinearGradientBrush();
            brush.StartPoint = startPoint;
            brush.EndPoint = endPoint;
            foreach (var (offset, color) in gradientStops)
                brush.ColorStops.Add(compositor.CreateColorGradientStop(offset, color));

            logger.Log("Created compositon linear gradient brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create compositon linear gradient brush", ex);
            return null;
        }
    }

    public CompositionMaskBrush? CretaeMaskBrush(Compositor compositor, CompositionBrush? source, CompositionBrush? mask)
    {
        try
        {
            var brush = compositor.CreateMaskBrush();
            brush.Source = source;
            brush.Mask = mask;

            logger.Log("Created Compositon Mask Brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create Compositon Mask Brush", ex);
            return null;
        }
    }

    public CompositionSurfaceBrush? CreateImageBrush(Compositor compositor, string source, CompositionStretch stretch)
    {
        try
        {
            var brush = compositor.CreateSurfaceBrush(LoadedImageSurface.StartLoadFromUri(source.FromAssets()));
            brush.Stretch = stretch;

            logger.Log("Created compositon surface (image) brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create compositon surface (image) brush", ex);
            return null;
        }
    }

    public SpriteVisual? CreateSpriteVisual(Compositor compositor, Vector2 size, CompositionBrush? brush)
    {
        try
        {
            var visual = compositor.CreateSpriteVisual();
            visual.Brush = brush;
            visual.Size = size;

            logger.Log("Created sprite visual");
            return visual;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create sprite visual", ex);
            return null;
        }
    }
}