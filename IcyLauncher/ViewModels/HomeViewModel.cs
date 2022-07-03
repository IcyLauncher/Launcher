using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Numerics;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Hosting;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    ILogger logger;

    public HomeViewModel(ILogger<HomeViewModel> logger)
    { 
        this.logger = logger;
    }

    [ObservableProperty]
    int selectedBannerImage;

    partial void OnSelectedBannerImageChanged(int value)
    {
        switch (value)
        {
            case 0:
                BannerSource = null;
                break;
            case 1:
                BannerSource = "Banners/TimeDependent/Snowy Forest/0.png".FromAssets();
                break;
            case 2:
                BannerSource = "Banners/TimeDependent/Snowy Forest/3.png".FromAssets();
                break;
            case 3:
                BannerSource = "Banners/TimeDependent/Snowy Forest/6.png".FromAssets();
                break;
            case 4:
                BannerSource = "Banners/TimeDependent/Snowy Forest/9.png".FromAssets();
                break;
            case 5:
                BannerSource = "Banners/TimeDependent/Snowy Forest/12.png".FromAssets();
                break;
            case 6:
                BannerSource = "Banners/TimeDependent/Snowy Forest/14.png".FromAssets();
                break;
            case 7:
                BannerSource = "Banners/TimeDependent/Snowy Forest/17.png".FromAssets();
                break;
            case 8:
                BannerSource = "Banners/TimeDependent/Snowy Forest/20.png".FromAssets();
                break;
        }
    }


    ContainerVisual banner = default!;
    Compositor bannerCompositor = default!;
    CompositionMaskBrush bannerMaskBrush = default!;
    SpriteVisual bannerMaskVisual = default!;

    public void OnBannerLoaded(object sender, RoutedEventArgs e)
    {
        if (banner is not null)
            return;

        var grid = (Grid)sender;
        grid.SizeChanged += (s, e) =>
            bannerMaskVisual.Size = new(e.NewSize._width, e.NewSize._height);

        banner = ElementCompositionPreview.GetElementVisual(grid).Compositor.CreateContainerVisual();
        ElementCompositionPreview.SetElementChildVisual(grid, banner);
        bannerCompositor = banner.Compositor;

        var bannerSourceGradient = bannerCompositor.CreateLinearGradientBrush();
        bannerSourceGradient.StartPoint = new(0, 0);
        bannerSourceGradient.EndPoint = new(0, 1);
        bannerSourceGradient.ColorStops.Add(bannerCompositor.CreateColorGradientStop(0.5f, Colors.White));
        bannerSourceGradient.ColorStops.Add(bannerCompositor.CreateColorGradientStop(1.0f, Colors.Transparent));

        bannerMaskBrush = bannerCompositor.CreateMaskBrush();
        bannerMaskBrush.Mask = bannerSourceGradient;

        bannerMaskVisual = bannerCompositor.CreateSpriteVisual();
        bannerMaskVisual.Brush = bannerMaskBrush;
        bannerMaskVisual.Size = new(946, 243);

        banner.Children.InsertAtTop(bannerMaskVisual);

        logger.Log("Initialized banner image composition");
    }

    Uri? bannerSource = null;
    public Uri? BannerSource
    {
        get => bannerSource;
        set
        {
            SetProperty(ref bannerSource, value);

            if (value is null)
            {
                bannerMaskBrush.Source = null;

                logger.Log("Removed banner image composition");
                return;
            }

            var bannerImageBrush = bannerCompositor.CreateSurfaceBrush(LoadedImageSurface.StartLoadFromUri(value));
            bannerImageBrush.Stretch = CompositionStretch.UniformToFill;
            bannerMaskBrush.Source = bannerImageBrush;

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