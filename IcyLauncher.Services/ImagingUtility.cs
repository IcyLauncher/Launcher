using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using System.Numerics;
using Windows.UI;

namespace IcyLauncher.Services;

public class ImagingUtility
{
    readonly ILogger<ImagingUtility> logger;

    /// <summary>
    /// Utility to render and image advanced UI
    /// </summary>
    public ImagingUtility(
        ILogger<ImagingUtility> logger)
    {
        this.logger = logger;

        logger.Log("Registered imaging utility");
    }


    /// <summary>
    /// Initializes the given UIElement
    /// </summary>
    /// <param name="element">The UIElement which should get initialized</param>
    /// <param name="compositor">The returned compositor of the UIElement</param>
    /// <param name="container">The returned container visual of the UIElement</param>
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

            logger.Log("Initialized UIElement compositor and container");
        }
        catch (Exception ex)
        {
            container = null;
            compositor = null;

            logger.Log("Failed to initialize UIElement compositor and comntainer", ex);
        }
    }

    /// <summary>
    /// Creates a new gradient brush
    /// </summary>
    /// <param name="compositor">The compositor which is used to create this composition object</param>
    /// <param name="startPoint">The start point of the gradient</param>
    /// <param name="endPoint">The end point of the gradient</param>
    /// <param name="gradientStops">The stop collection of the gradient</param>
    /// <returns>A new composition brush which will have the gradient applied</returns>
    public CompositionLinearGradientBrush? CreateGradientBrush(
        Compositor compositor,
        Vector2 startPoint,
        Vector2 endPoint,
        (float offset, Color color)[] gradientStops)
    {
        try
        {
            CompositionLinearGradientBrush brush = compositor.CreateLinearGradientBrush();
            brush.StartPoint = startPoint;
            brush.EndPoint = endPoint;

            foreach ((float offset, Color color) in gradientStops)
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

    /// <summary>
    /// Creates a new opacity mask brush
    /// </summary>
    /// <param name="compositor">The compositor which is used to create this composition object</param>
    /// <param name="source">The brush which will be used as the source of the new brush</param>
    /// <param name="mask">The brush which will be uses as the mask</param>
    /// <returns>A new compostion brush which will have the mask applied</returns>
    public CompositionMaskBrush? CreateMaskBrush(
        Compositor compositor,
        CompositionBrush? source,
        CompositionBrush? mask)
    {
        try
        {
            CompositionMaskBrush brush = compositor.CreateMaskBrush();
            brush.Source = source;
            brush.Mask = mask;

            logger.Log("Created compositon mask brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create compositon mask brush", ex);
            return null;
        }
    }

    /// <summary>
    /// Creates a new surface brush with a image
    /// </summary>
    /// <param name="compositor">The compositor which is used to create this composition object</param>
    /// <param name="source">The path to the image which will be used</param>
    /// <param name="stretch">The stretch mode which will be aplied to the brush</param>
    /// <param name="horizontalAlignmentRatio">the horizontal alignment which will be aplied to the brush</param>
    /// <param name="verticalAlignmentRatio">The vertical alignment which will be applied to the brush</param>
    /// <returns>A new compostion brush which will have the image applied</returns>
    public CompositionSurfaceBrush? CreateImageBrush(
        Compositor compositor,
        Uri source,
        CompositionStretch stretch,
        float horizontalAlignmentRatio = 0.0f,
        float verticalAlignmentRatio = 0.5f)
    {
        try
        {
            CompositionSurfaceBrush brush = compositor.CreateSurfaceBrush(LoadedImageSurface.StartLoadFromUri(source));
            brush.Stretch = stretch;
            brush.HorizontalAlignmentRatio = horizontalAlignmentRatio;
            brush.VerticalAlignmentRatio = verticalAlignmentRatio;

            logger.Log("Created compositon image brush");
            return brush;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to create compositon image brush", ex);
            return null;
        }
    }

    /// <summary>
    /// Creates a new color brush
    /// </summary>
    /// <param name="compositor">The compositor which is used to create this composition object</param>
    /// <param name="color">The color which will be aplied as the color brush</param>
    /// <returns>A new compostion brush which will have the color applied</returns>
    public CompositionColorBrush? CreateColorBrush(
        Compositor compositor,
        Color color)
    {
        try
        {
            CompositionColorBrush brush = compositor.CreateColorBrush(color);

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
            SpriteVisual visual = compositor.CreateSpriteVisual();
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