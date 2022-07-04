using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Composition;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger<HomeViewModel> logger;
    readonly ImagingUtility imagingUtility;

    public HomeViewModel(ILogger<HomeViewModel> logger, ImagingUtility imagingUtility)
    { 
        this.logger = logger;
        this.imagingUtility = imagingUtility;
    }


    public void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        BannerSource = "Banners/TimeDependent/Icy Village/12.png";
    }

    Compositor? bannerCompositor;
    CompositionMaskBrush? bannerMaskBrush;
    SpriteVisual? bannerMaskVisual;

    public void OnBannerLoaded(object sender, RoutedEventArgs e)
    {
        if (bannerMaskVisual is not null)
            return;

        var grid = (Grid)sender;
        grid.SizeChanged += (s, e) =>
        {
            if (bannerMaskVisual is not null)
                bannerMaskVisual.Size = new(e.NewSize._width, e.NewSize._height);
        };

        imagingUtility.InitializeUIElement(grid, out bannerCompositor, out var banner);

        if (bannerCompositor is null || banner is null)
            return;

        bannerMaskBrush = imagingUtility.CretaeMaskBrush(
            bannerCompositor,
            null, imagingUtility.CreateGradientBrush(
                bannerCompositor,
                new(0, 0), new(0, 1),
                new[] { (0.5f, Colors.White), (1.0f, Colors.Transparent) }));
        bannerMaskVisual = imagingUtility.CreateSpriteVisual(
            bannerCompositor,
            new(946, 243), bannerMaskBrush);

        banner.Children.InsertAtTop(bannerMaskVisual);

        logger.Log("Initialized banner image composition");
    }

    string? bannerSource;
    public string? BannerSource
    {
        get => bannerSource;
        set
        {
            if (bannerMaskBrush is null || bannerCompositor is null)
            {
                logger.Log("Tried to update banner image composition", Exceptions.IsNull);
                return;
            }
            if (BannerSource == value)
                return;

            SetProperty(ref bannerSource, value);

            bannerMaskBrush.Source = value is null ? null : imagingUtility.CreateImageBrush(bannerCompositor, value, CompositionStretch.UniformToFill);
            logger.Log("Updated banner image composition");
        }
    }


    public void OnItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.RemovedItems.Count == 0)
            return;

        var rootLayout = (Grid)((GridViewItem)((GridView)sender).ContainerFromItem(e.RemovedItems[0])).ContentTemplateRoot;
        var details = rootLayout.Children[4];
        var icon = (Image)rootLayout.Children[2];

        details.Opacity = 0;
        details.Translation = new(-10, 0, 0);
        ((Storyboard)icon.Resources["outBoard"]).Begin();
    }
}