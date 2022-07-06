using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Composition;
using Windows.UI;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger<HomeViewModel> logger;
    readonly ImagingUtility imagingUtility;
    readonly ThemeManager themeManager;

    public HomeViewModel(ILogger<HomeViewModel> logger, ImagingUtility imagingUtility, ThemeManager themeManager)
    { 
        this.logger = logger;
        this.imagingUtility = imagingUtility;
        this.themeManager = themeManager;

        this.themeManager.Colors.Background.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case "Gradient":
                case "GradientTransparent":
                    if (bannerOverlayBrush is not null && bannerCompositor is not null)
                        bannerOverlayBrush.Source = imagingUtility.CreateGradientBrush(bannerCompositor,
                            new(0, 0), new(1, 0),
                            new[] { (0f, themeManager.Colors.Background.Gradient), (1f, themeManager.Colors.Background.GradientTransparent) });

                    logger.Log("Updated banner image (overlay) composition colors");
                    break;
            }
        };
    }


    public void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        BannerSource = "Banners/TimeDependent/Icy Village/17.png";
    }

    Compositor? bannerCompositor;
    CompositionLinearGradientBrush? maskOverlayBrush;
    CompositionMaskBrush? bannerMaskBrush;
    SpriteVisual? bannerMaskVisual;
    CompositionMaskBrush? bannerOverlayBrush;

    public void OnBannerLoaded(object sender, RoutedEventArgs e)
    {
        if (bannerMaskVisual is not null)
            return;

        var grid = (Rectangle)sender;
        grid.SizeChanged += (s, e) =>
        {
            if (bannerMaskVisual is not null)
                bannerMaskVisual.Size = new(e.NewSize._width, e.NewSize._height);
        };


        imagingUtility.InitializeUIElement(grid, out bannerCompositor, out var banner);

        if (bannerCompositor is null || banner is null)
            return;

        maskOverlayBrush = imagingUtility.CreateGradientBrush(
                bannerCompositor,
                new(0, 0), new(0, 1),
                new[] { (0.5f, Colors.White), (1.0f, Colors.Transparent) });

        bannerMaskBrush = imagingUtility.CretaeMaskBrush(bannerCompositor, null, maskOverlayBrush);
        bannerMaskVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(946, 243), bannerMaskBrush);

        bannerOverlayBrush = imagingUtility.CretaeMaskBrush(bannerCompositor, imagingUtility.CreateGradientBrush(bannerCompositor,
                new(0, 0), new(1, 0),
                new[] { (-0.2f, themeManager.Colors.Background.Gradient), (1f, themeManager.Colors.Background.GradientTransparent) }), maskOverlayBrush);
        var bannerOverlayVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(500, 243), bannerOverlayBrush);


        banner.Children.InsertAtTop(bannerMaskVisual);
        banner.Children.InsertAtTop(bannerOverlayVisual);

        logger.Log("Initialized banner image composition");
    }

    string? bannerSource;
    public string? BannerSource
    {
        get => bannerSource;
        set
        {
            if (BannerSource == value)
                return;
            if (bannerMaskBrush is null || bannerCompositor is null)
            {
                logger.Log("Tried to update banner image composition", Exceptions.IsNull);
                return;
            }

            SetProperty(ref bannerSource, value);

            bannerMaskBrush.Source = value is null ? null : imagingUtility.CreateImageBrush(bannerCompositor, value, CompositionStretch.UniformToFill);
            logger.Log("Updated banner image composition");
        }
    }


    [ObservableProperty]
    Profile selectedProfile = new();

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