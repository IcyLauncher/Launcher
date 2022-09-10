using IcyLauncher.WinUI.DataTemplates;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger<HomeViewModel> logger;
    readonly SolidColorCollection solidColors;
    readonly ThemeManager themeManager;
    readonly ImagingUtility imagingUtility;

    public readonly Configuration Configuration;
    public readonly Updater Updater;

    public HomeViewModel(
        ILogger<HomeViewModel> logger,
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ThemeManager themeManager,
        ImagingUtility imagingUtility,
        Updater updater)
    {
        this.logger = logger;
        this.solidColors = solidColors.Value;
        this.themeManager = themeManager;
        this.imagingUtility = imagingUtility;

        Configuration = configuration.Value;
        Updater = updater;

        themeManager.Colors.Background.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case "Gradient":
                case "GradientTransparent":
                    if (bannerOverlayBrush is not null && bannerCompositor is not null)
                        bannerOverlayBrush.Source = imagingUtility.CreateGradientBrush(bannerCompositor,
                            new(0, 0), new(1, 0),
                            new[] { (0f, themeManager.Colors.Background.Gradient), (1f, themeManager.Colors.Background.GradientTransparent) });

                    logger.Log("Updated banner image composition [overlay]");
                    break;
            }
        };
    }


    readonly List<string> bannerCustomPictures = new();
    readonly List<BannerTimeDependentItem> bannerTimeDependentItems = new();

    public void OnPageLoaded(object sender, RoutedEventArgs? _)
    {
        if (bannerProfileGrid is null)
            bannerProfileGrid = (Grid)((Grid)((Grid)((Page)sender).Content).Children[0]).Children[2];

        if (bannerProfileInBoard is null)
        {
            bannerProfileInBoard = new();
            bannerProfileInBoard.Children.Add(UIElementProvider.Animate(bannerProfileGrid, "Opacity", null, 1, 100));
        }

        if (SelectedProfile is null)
            SelectedProfile = Profiles.First();

        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                bannerTimeDependentItems.Clear();

                // API STUFF => API NOT DONE: LOCAL TIME DEPENDENT ITEMS
                bannerTimeDependentItems.Add(new("Icy Village",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/0.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/3.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/6.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/9.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/12.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/15.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/18.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Icy Village/21.png"));
                bannerTimeDependentItems.Add(new("Showy Forest",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/0.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/3.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/6.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/9.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/12.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/15.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/18.png",
                    "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/21.png"));

                logger.Log("Reloaded TimeDependentItems");

                if (Configuration.Apperance.SelectedHomeBanner < 0 || Configuration.Apperance.SelectedHomeBanner >= bannerTimeDependentItems.Count)
                {
                    BannerSource = "Banners/NoBanner.png".FromAssets();
                    return;
                }

                BannerSource = new(DateTime.Now.Hour.RoundDown(new[] { 0, 3, 6, 9, 12, 15, 18, 21 }) switch
                {
                    0 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_0,
                    3 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_3,
                    6 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_6,
                    9 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_9,
                    12 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_12,
                    15 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_15,
                    18 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_18,
                    21 => bannerTimeDependentItems[Configuration.Apperance.SelectedHomeBanner].I_21,
                    _ => "ms-appx:///Assets/Banners/NoBanner.png"
                });
                break;
            case BannerType.Gallery:
                BannerSource = Uri.IsWellFormedUriString(Configuration.Apperance.HomeBannerUri, UriKind.Absolute) ? new(Configuration.Apperance.HomeBannerUri) : "Banners/NoBanner.png".FromAssets();
                break;
            case BannerType.CustomPicture:
                bannerCustomPictures.Clear();

                if (Directory.Exists("Assets\\Banners\\Custom"))
                    foreach (string img in Directory.GetFiles("Assets\\Banners\\Custom").Where(path =>
                        path.EndsWith(".jpg") ||
                        path.EndsWith(".jpeg") ||
                        path.EndsWith(".png")))
                        bannerCustomPictures.Add(img);

                logger.Log("Reloaded CustomPictures");

                if (Configuration.Apperance.SelectedHomeBanner < 0 || Configuration.Apperance.SelectedHomeBanner >= bannerCustomPictures.Count)
                {
                    BannerSource = "Banners/NoBanner.png".FromAssets();
                    return;
                }

                BannerSource = new(System.IO.Path.Combine(Computer.CurrentDirectory, bannerCustomPictures[Configuration.Apperance.SelectedHomeBanner]));
                break;
            case BannerType.SolidColor:
                if (Configuration.Apperance.SelectedHomeBanner < 0 || Configuration.Apperance.SelectedHomeBanner >= solidColors.Container.Count)
                {
                    BannerSource = "Banners/NoBanner.png".FromAssets();
                    return;
                }

                BannerColor = solidColors.Container[Configuration.Apperance.SelectedHomeBanner].Color;
                break;
        };
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

        Rectangle grid = (Rectangle)sender;
        grid.SizeChanged += (s, e) =>
        {
            if (bannerMaskVisual is not null)
                bannerMaskVisual.Size = new(e.NewSize._width, e.NewSize._height);
        };


        imagingUtility.InitializeUIElement(grid, out bannerCompositor, out ContainerVisual? banner);

        if (bannerCompositor is null || banner is null)
            return;

        maskOverlayBrush = imagingUtility.CreateGradientBrush(
                bannerCompositor,
                new(0, 0), new(0, 1),
                new[] { (0.5f, Colors.Gray), (1.0f, Colors.Transparent) });

        bannerMaskBrush = imagingUtility.CreateMaskBrush(bannerCompositor, null, maskOverlayBrush);
        bannerMaskVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new((float)grid.ActualWidth, (float)grid.ActualHeight), bannerMaskBrush);

        bannerOverlayBrush = imagingUtility.CreateMaskBrush(bannerCompositor, imagingUtility.CreateGradientBrush(bannerCompositor,
                new(0, 0), new(1, 0),
                new[] { (-0.2f, themeManager.Colors.Background.Gradient), (1f, themeManager.Colors.Background.GradientTransparent) }), maskOverlayBrush);
        SpriteVisual? bannerOverlayVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(500, 330), bannerOverlayBrush);


        banner.Children.InsertAtTop(bannerMaskVisual);
        banner.Children.InsertAtTop(bannerOverlayVisual);

        logger.Log("Initialized banner image composition");
    }

    Uri? bannerSource;
    public Uri? BannerSource
    {
        get => bannerSource;
        set
        {
            if (value == BannerSource)
                return;
            if (bannerMaskBrush is null || bannerCompositor is null)
            {
                logger.Log("Failed to update banner image composition", Exceptions.IsNull);
                return;
            }

            SetProperty(ref bannerSource, value);

            bannerMaskBrush.Source = value is null ? null : imagingUtility.CreateImageBrush(bannerCompositor, value, CompositionStretch.UniformToFill, 0.5f);
            logger.Log("Updated banner image composition");
        }
    }

    Color? bannerColor;
    public Color? BannerColor
    {
        get => bannerColor;
        set
        {
            if (BannerColor == value)
                return;
            if (bannerMaskBrush is null || bannerCompositor is null)
            {
                logger.Log("Failed to update banner color composition", Exceptions.IsNull);
                return;
            }

            SetProperty(ref bannerColor, value);

            bannerMaskBrush.Source = value is null ? null : imagingUtility.CreateColorBrush(bannerCompositor, value.Value);
            logger.Log("Updated banner color composition");
        }
    }


    [ObservableProperty]
    int launchProgress;

    [ObservableProperty]
    string launchProgressDetails = "Launch";

    [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(LaunchCanExecute), FlowExceptionsToTaskScheduler = true, IncludeCancelCommand = true)]
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
        new("RTX", Colors.Black, "Blocks/Glass.png".AsImage(), "1.16.1", "Vanilla")
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
        Configuration.Weather.Unit == WeatherUnit.Celsius ? $"{degree} °C" : $"{degree * 1.8 + 32} °F";

    [RelayCommand]
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