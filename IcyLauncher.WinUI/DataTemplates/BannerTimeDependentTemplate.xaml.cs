using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.WinUI.DataTemplates;

public partial class BannerTimeDependentTemplate : ResourceDictionary
{
    #region Setup
    public BannerTimeDependentTemplate() =>
        InitializeComponent();


    ImagingUtility imagingUtility = default!;
    #endregion


    #region Handlers
    void OnImageLoaded(object sender, RoutedEventArgs _)
    {
        if (imagingUtility is null)
            imagingUtility = App.Provider.GetRequiredService<ImagingUtility>();

        Rectangle image = (Rectangle)sender;
        BannerTimeDependentPack pack = (BannerTimeDependentPack)image.DataContext;


        imagingUtility.InitializeUIElement(image, out Compositor? bannerCompositor, out ContainerVisual? banner);

        if (bannerCompositor is null || banner is null)
            return;

        for (int i = 1; i < pack.Collection.Count; i++)
            AddImagePart(banner, bannerCompositor, pack.Collection[i].Image, i, pack.Collection.Count);

        CompositionSurfaceBrush? backgroundBrush = imagingUtility.CreateImageBrush(bannerCompositor, new(pack.Collection[0].Image), CompositionStretch.UniformToFill);
        SpriteVisual? backgroundVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(270f, 66f), backgroundBrush);

        banner.Children.InsertAtBottom(backgroundVisual);
    }
    #endregion

    #region Compostion
    void AddImagePart(
        ContainerVisual container,
        Compositor compositor,
        string image,
        int index,
        int max)
    {
        CompositionBrush? mask = imagingUtility.CreateGradientBrush(
            compositor, new(0f, 0f), new(1f, 0f), new[] { (0f, Colors.Transparent), (0.15f * index, Colors.White) });

        CompositionBrush? imageBrush = imagingUtility.CreateImageBrush(
            compositor, new(image), CompositionStretch.UniformToFill, 1f);

        CompositionBrush? maskBrush = imagingUtility.CreateMaskBrush(
            compositor, imageBrush, mask);


        float onePart = 270f / max;
        float currentPart = onePart * index - 20f;

        SpriteVisual? visual = imagingUtility.CreateSpriteVisual(
            compositor, new(270f - currentPart, 66f), maskBrush, new(currentPart, 0f, 0f));

        container.Children.InsertAtTop(visual);
    }
    #endregion
}