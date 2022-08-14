using IcyLauncher.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.UI.Xaml;

public partial class BannerTimeDependentTemplate : ResourceDictionary
{
    public BannerTimeDependentTemplate() =>
        InitializeComponent();


    ImagingUtility imagingUtility = default!;


    private void OnImageLoaded(object sender, RoutedEventArgs e)
    {
        if (imagingUtility is null)
            imagingUtility = App.Provider.GetRequiredService<ImagingUtility>();

        var grid = (Rectangle)sender;
        var timeDependent = (BannerTimeDependentItem)grid.DataContext;


        imagingUtility.InitializeUIElement(grid, out var bannerCompositor, out var banner);

        if (bannerCompositor is null || banner is null)
            return;

        AddImagePart(banner, bannerCompositor, timeDependent.I_3, 1);
        AddImagePart(banner, bannerCompositor, timeDependent.I_6, 2);
        AddImagePart(banner, bannerCompositor, timeDependent.I_9, 3);
        AddImagePart(banner, bannerCompositor, timeDependent.I_12, 4);
        AddImagePart(banner, bannerCompositor, timeDependent.I_15, 5);
        AddImagePart(banner, bannerCompositor, timeDependent.I_21, 7);

        var backgroundBrush = imagingUtility.CreateImageBrush(bannerCompositor, new(timeDependent.I_0), CompositionStretch.UniformToFill);
        var backgroundVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(270f, 66f), backgroundBrush);

        banner.Children.InsertAtBottom(backgroundVisual);
    }

    void AddImagePart(ContainerVisual container, Compositor compositor, string image, int index) =>
        container.Children.InsertAtTop(imagingUtility.CreateSpriteVisual(compositor,
            new(286.875f - (33.75f * index), 66f),
            imagingUtility.CreateMaskBrush(compositor,
                imagingUtility.CreateImageBrush(compositor, new(image), CompositionStretch.UniformToFill, 1f),
                imagingUtility.CreateGradientBrush(
                    compositor,
                    new(0f, 0f), new(1f, 0f),
                    new[] { (0f, Colors.Transparent), (10f * index / 100, Colors.White) })),
            new((33.75f * index) - 16.875f, 0f, 0f)));
}