using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Shapes;
using System.Collections.ObjectModel;
using IcyLauncher.UI.Xaml;
using System.Numerics;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly Configuration configuration;
    readonly ILogger<HomeViewModel> logger;
    readonly ImagingUtility imagingUtility;
    readonly ThemeManager themeManager;

    public HomeViewModel(IOptions<Configuration> configuration, ILogger<HomeViewModel> logger, ImagingUtility imagingUtility, ThemeManager themeManager)
    {
        this.configuration = configuration.Value;
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
            bannerProfileInBoard = new();
            bannerProfileInBoard.Children.Add(UIElementProvider.Animate(bannerProfileGrid, "Opacity", null, 1, 100));
        }

        if (SelectedProfile is null)
            SelectedProfile = Profiles.First();

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


    [ObservableProperty]
    int launchProgress;

    [ObservableProperty]
    string launchProgressDetails = "Launch";

    [ICommand(AllowConcurrentExecutions = false, CanExecute = nameof(LaunchCanExecute), IncludeCancelCommand = true)]
    async Task Launch(CancellationToken cancellationToken)
    {
        cancellationToken.Register(async () =>
        {
            LaunchProgress = 0;
            LaunchProgressDetails = "Canceling...";

            for (int i = 0; i < 5; i++)
            {
                LaunchProgress += 20;
                await Task.Delay(1000);
            }

            LaunchProgress = 0;
            LaunchProgressDetails = "Launch";
            LaunchCommand.NotifyCanExecuteChanged();
        });

        LaunchProgress = 0;
        LaunchProgressDetails = "Launching...";

        for (int i = 0; i < 5; i++)
        {
            LaunchProgress += 20;
            await Task.Delay(1000, cancellationToken);
        }

        LaunchProgress = 0;
        LaunchProgressDetails = "Launched!";
    }

    bool LaunchCanExecute()
    {
        return SelectedProfile is not null && LaunchProgressDetails == "Launch";
    }


    Grid bannerProfileGrid = default!;
    Storyboard bannerProfileInBoard = default!;

    [ObservableProperty]
    ObservableCollection<Profile> profiles = new()
    {
        new("Latest Release", Colors.White, "Blocks/Grass_Block.png".AsImage(), "1.19", "Vanilla"),
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
                if (SetProperty(ref selectedProfile, value))
                {
                    BannerProfileDetails = value is null ? "Select a profile to start playing!" : $"Version {value.Version} | {value.Client}";
                    LaunchCommand.NotifyCanExecuteChanged();
                }

                bannerProfileGrid.Translation = new Vector3(0, 0, 0);
                bannerProfileInBoard.Begin();
            };
            bannerProfileGrid.Translation = new Vector3(-10, 0, 0);
            outBoard.Begin();
        }
    }

    [ObservableProperty]
    string bannerProfileDetails = "Select a profile to start playing!";

    public void OnProfileSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count != 0)
            ProfileTemplate.UpdateProperties((GridView)sender, e.AddedItems[0], 1, new(0, 0, 0), "inBoard");
        if (e.RemovedItems.Count != 0)
            ProfileTemplate.UpdateProperties((GridView)sender, e.RemovedItems[0], 0, new(-10, 0, 0), "outBoard");
    }


    [ObservableProperty]
    string[] news = new[]
    {
        "This is an example news [1]",
        "This is another example [2]",
        "And again, a fucking example news [3]",
        "End my suffer. Please. I cant do this anymore"
    };


    [ObservableProperty]
    WeatherModel weatherData = new();

    public string FormatDegree(double degree) =>
        configuration.Weather.Unit == WeatherUnit.Celsius ? $"{degree} °C" : $"{degree * 1.8 + 32} °F";

    [ICommand]
    void NavigateToWeather()
    {
        WeatherData = new()
        {
            City = "Miami",
            State = "Florida",
            Country = "USA",
            Recieved = DateTime.UtcNow,
            Degree = 32,
            Icon = "Weather/02d.png".AsImage(),
            Description = "mostly sunny"
        };
    }
}