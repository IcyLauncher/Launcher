using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using System.Numerics;
using Windows.UI;

namespace IcyLauncher.Services;

public class ImagingUtility
{
    readonly ILogger<ImagingUtility> logger;

    public ImagingUtility(
        ILogger<ImagingUtility> logger)
    {
        this.logger = logger;

        logger.Log("Registered ImagingUtility");
    }


    public void InitializeUIElement(
        UIElement element,
        out Compositor? compositor,
        out ContainerVisual? container)
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

    public CompositionLinearGradientBrush? CreateGradientBrush(
        Compositor compositor,
        Vector2 startPoint,
        Vector2 endPoint,
        (float offset, Color color)[] gradientStops)
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

    public CompositionMaskBrush? CreateMaskBrush(
        Compositor compositor,
        CompositionBrush? source,
        CompositionBrush? mask)
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

    public CompositionSurfaceBrush? CreateImageBrush(
        Compositor compositor,
        Uri source,
        CompositionStretch stretch,
        float horizontalAlignmentRatio = 0.0f,
        float verticalAlignmentRatio = 0.5f)
    {
        try
        {
            var brush = compositor.CreateSurfaceBrush(LoadedImageSurface.StartLoadFromUri(source));
            brush.Stretch = stretch;
            brush.HorizontalAlignmentRatio = horizontalAlignmentRatio;
            brush.VerticalAlignmentRatio = verticalAlignmentRatio;

            logger.Log("Created compositon surface (image) brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create compositon surface (image) brush", ex);
            return null;
        }
    }

    public CompositionColorBrush? CreateColorBrush(
        Compositor compositor,
        Color color)
    {
        try
        {
            var brush = compositor.CreateColorBrush(color);

            logger.Log("Created compositon color brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create compositon color brush", ex);
            return null;
        }
    }

    public SpriteVisual? CreateSpriteVisual(
        Compositor compositor,
        Vector2 size,
        CompositionBrush? brush,
        Vector3 offset = default)
    {
        try
        {
            var visual = compositor.CreateSpriteVisual();
            visual.Brush = brush;
            visual.Size = size;
            visual.Offset = offset;

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