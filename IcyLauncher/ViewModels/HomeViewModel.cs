using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Composition;
using Windows.UI;
using Microsoft.UI.Xaml.Shapes;
using System.Collections.ObjectModel;
using IcyLauncher.UI;
using System.Numerics;

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


    public void OnPageLoaded(object sender, RoutedEventArgs? _)
    {
        bannerProfileGrid = (Grid)((Grid)((Grid)((Page)sender).Content).Children[0]).Children[2];

        if (bannerProfileInBoard is null)
        {
            bannerProfileInBoard = new() ;
            bannerProfileInBoard.Children.Add(UIElementProvider.Animate(bannerProfileGrid, "Opacity", null, 1, 100));
        }

        BannerSource = "Banners/TimeDependent/Icy Village/17.png";
    }

    Compositor? bannerCompositor;
    CompositionLinearGradientBrush? maskOverlayBrush;
    CompositionMaskBrush? bannerMaskBrush;
    SpriteVisual? bannerMaskVisual;
    CompositionMaskBrush? bannerOverlayBrush;

    public void OnBannerLoaded(object sender, RoutedEventArgs? _)
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


    Grid bannerProfileGrid = default!;
    Storyboard bannerProfileInBoard = default!;

    [ObservableProperty]
    ObservableCollection<Profile> profiles = new()
    {
        new("Default Profile", Colors.White, "Blocks/Grass_Block.png".AsImage(), "1.19", "Vanilla"),
        new("Beta", Colors.Red, "Blocks/Block_Of_Redstone.png".AsImage(), "1.18.2.3-beta", "Vanilla"),
        new("PvP Profile", Colors.Aqua, "Blocks/Block_Of_Diamond.png".AsImage(), "1.17.1", "OnixClient"),
        new("DebugTesting", Colors.GreenYellow, "Blocks/Slime_Block.png".AsImage(), "1.16.4", "MyCustomClient.dll"),
        new("Survival SMP", Colors.Yellow, "Blocks/Furnace.png".AsImage(), "1.18", "Horion"),
        new("RTX", Colors.Black, "Blocks/Glass.png".AsImage(), "1.16.1", "Vanilla"),
    };

    Profile? selectedProfile;
    public Profile? SelectedProfile
    {
        get
        {
            return selectedProfile;
        }
        set
        {
            Storyboard outBoard = new();
            outBoard.Children.Add(UIElementProvider.Animate(bannerProfileGrid, "Opacity", null, 0, 100));
            outBoard.Completed += (s, e) =>
            {
                SetProperty(ref selectedProfile, value);
                bannerProfileGrid.Translation = new Vector3(0, 0, 0);
                bannerProfileInBoard.Begin();
            };
            bannerProfileGrid.Translation = new Vector3(-10, 0, 0);
            outBoard.Begin();
        }
    }

    public void OnProfileSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count != 0)
            ProfileTemplate.UpdateProperties((GridView)sender, e.AddedItems[0], 1, new(0, 0, 0), "inBoard");
        if (e.RemovedItems.Count != 0)
            ProfileTemplate.UpdateProperties((GridView)sender, e.RemovedItems[0], 0, new(-10, 0, 0), "outBoard");
    }

    [ICommand]
    void Sexx()
    {
        SelectedProfile = Profiles.First();
    }
}