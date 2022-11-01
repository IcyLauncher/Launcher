using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    #region Setup
    readonly ILogger<HomeViewModel> logger;
    readonly SolidColorCollection solidColors;
    readonly ThemeManager themeManager;
    readonly ImagingUtility imagingUtility;
    readonly FeedbackRequest feedbackRequest;
    readonly IMessage message;

    public readonly Configuration Configuration;
    public readonly Updater Updater;

    public HomeViewModel(
        ILogger<HomeViewModel> logger,
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ThemeManager themeManager,
        ImagingUtility imagingUtility,
        FeedbackRequest feedbackRequest,
        Updater updater,
        IMessage message)
    {
        this.logger = logger;
        this.solidColors = solidColors.Value;
        this.themeManager = themeManager;
        this.imagingUtility = imagingUtility;
        this.feedbackRequest = feedbackRequest;
        this.message = message;

        Configuration = configuration.Value;
        Updater = updater;

        SetupViewModel();
    }

    void SetupViewModel()
    {
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

    [RelayCommand]
    void SetupPage(
        Grid bannerContainer)
    {
        SetupBannerComposition((Rectangle)bannerContainer.Children[0]);
        SetupProfile((Grid)bannerContainer.Children[2]);
        LoadBannerImage();
    }
    #endregion


    #region Feedback
    public async Task AskForFeedback()
    {
        if (!feedbackRequest.RandomShouldShow)
            return;

        await Task.Delay(2000);
        Feedback result = await feedbackRequest.ShowAsync();

        if (result.Result == FeedbackResult.Submit)
            await feedbackRequest.SubmitAsync(result);
    }
    #endregion


    #region Banner Composition
    Compositor? bannerCompositor;
    CompositionLinearGradientBrush? maskOverlayBrush;
    CompositionMaskBrush? bannerMaskBrush;
    SpriteVisual? bannerMaskVisual;
    CompositionMaskBrush? bannerOverlayBrush;

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

    void SetupBannerComposition(
        Rectangle container)
    {
        container.SizeChanged += (s, e) =>
        {
            if (bannerMaskVisual is not null)
                bannerMaskVisual.Size = new(e.NewSize._width, e.NewSize._height);
        };


        imagingUtility.InitializeUIElement(container, out bannerCompositor, out ContainerVisual? banner);

        if (bannerCompositor is null || banner is null)
            return;

        maskOverlayBrush = imagingUtility.CreateGradientBrush(
                bannerCompositor,
                new(0, 0), new(0, 1),
                new[] { (0.5f, Colors.Gray), (1.0f, Colors.Transparent) });

        bannerMaskBrush = imagingUtility.CreateMaskBrush(bannerCompositor, null, maskOverlayBrush);
        bannerMaskVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new((float)container.ActualWidth, (float)container.ActualHeight), bannerMaskBrush);

        bannerOverlayBrush = imagingUtility.CreateMaskBrush(bannerCompositor, imagingUtility.CreateGradientBrush(bannerCompositor,
                new(0, 0), new(1, 0),
                new[] { (-0.2f, themeManager.Colors.Background.Gradient), (1f, themeManager.Colors.Background.GradientTransparent) }), maskOverlayBrush);
        SpriteVisual? bannerOverlayVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(500, 330), bannerOverlayBrush);


        banner.Children.InsertAtTop(bannerMaskVisual);
        banner.Children.InsertAtTop(bannerOverlayVisual);

        logger.Log("Initialized banner image composition");
    }
    #endregion


    #region Banner Image
    readonly List<string> bannerCustomPictures = new();
    readonly List<BannerTimeDependentPack> bannerTimeDependentPacks = new();

    public void LoadBannerImage()
    {
        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                bannerTimeDependentPacks.Clear();

                // API STUFF => API NOT DONE: LOCAL TIME DEPENDENT ITEMS
                bannerTimeDependentPacks.Add(new("Icy Village", new()
                {
                    new(0, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/0.png"),
                    new(3, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/3.png"),
                    new(6, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/6.png"),
                    new(9, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/9.png"),
                    new(12, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/12.png"),
                    new(15, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/15.png"),
                    new(18, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/18.png"),
                    new(21, "ms-appx:///Assets/Banners/TimeDependent/Icy Village/21.png")
                }));
                bannerTimeDependentPacks.Add(new("Snowy Forest", new()
                {
                    new(0, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/0.png"),
                    new(3, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/3.png"),
                    new(6, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/6.png"),
                    new(9, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/9.png"),
                    new(12, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/12.png"),
                    new(15, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/15.png"),
                    new(18, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/18.png"),
                    new(21, "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/21.png")
                }));

                logger.Log("Reloaded TimeDependentItems");

                if (Configuration.Apperance.SelectedHomeBanner < 0 || Configuration.Apperance.SelectedHomeBanner >= bannerTimeDependentPacks.Count)
                {
                    BannerSource = "Banners/NoBanner.png".FromAssets();
                    return;
                }

                IEnumerable<int> all = bannerTimeDependentPacks[Configuration.Apperance.SelectedHomeBanner].Collection.Select(item => item.Hour);
                int searchFor = DateTime.Now.Hour.RoundDown(all);
                var item = bannerTimeDependentPacks[Configuration.Apperance.SelectedHomeBanner].Collection.Find(item => item.Hour == searchFor);
                BannerSource = new(item?.Image ?? "ms-appx:///Assets/Banners/NoBanner.png");
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
    #endregion


    #region Launch
    [ObservableProperty]
    int launchProgress;

    [ObservableProperty]
    string launchProgressDetails = "Launch";

    [RelayCommand(CanExecute = nameof(LaunchCanExecute), FlowExceptionsToTaskScheduler = true, IncludeCancelCommand = true)]
    async Task Launch(
        CancellationToken cancellationToken)
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
    #endregion


    #region Profile
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
        get => selectedProfile;
        set
        {
            newProfile = value;
            bannerProfileGrid.Translation = new Vector3(-10, 0, 0);
            ((Storyboard)bannerProfileGrid.Resources["OutBoard"]).Begin();
        }
    }

    Profile? newProfile;

    public void UpdateProfile()
    {
        if (SetProperty(ref selectedProfile, newProfile, nameof(SelectedProfile)))
        {
            LaunchCommand.NotifyCanExecuteChanged();
        }
        bannerProfileGrid.Translation = new Vector3(0, 0, 0);
        bannerProfileInBoard.Begin();
    }


    void SetupProfile(
        Grid container)
    {
        bannerProfileGrid = container;
        bannerProfileInBoard = (Storyboard)bannerProfileGrid.Resources["InBoard"];

        SelectedProfile = Profiles.First();
    }
    #endregion


    #region News
    [ObservableProperty]
    string[] news = new[]
    {
        "This is an example news [1]",
        "This is another example [2]",
        "And again, a fucking example news [3]",
        "End my suffer. Please. I cant do this anymore"
    };
    #endregion


    #region Weather
    [ObservableProperty]
    WeatherModel weatherData = new();

    public string FormatDegree(
        double degree) =>
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
    #endregion
}