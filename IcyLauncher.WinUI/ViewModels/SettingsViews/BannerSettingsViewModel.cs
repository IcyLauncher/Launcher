using IcyLauncher.Xaml.Elements;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels;

public partial class BannerSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly WindowHandler windowHandler;
    readonly IFileSystem fileSystem;
    readonly IMessage message;
    readonly INavigation navigation;

    public readonly Configuration Configuration;
    public readonly SolidColorCollection SolidColors;

    public BannerSettingsViewModel(
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ILogger<ProfilesViewModel> logger,
        WindowHandler windowHandler,
        IFileSystem fileSystem,
        IMessage message,
        INavigation navigation)
    {
        this.logger = logger;
        this.windowHandler = windowHandler;
        this.fileSystem = fileSystem;
        this.message = message;
        this.navigation = navigation;

        Configuration = configuration.Value;
        SolidColors = solidColors.Value;


        windowHandler.Register(filePicker);
        filePicker.FileTypeFilter.Add(".jpg");
        filePicker.FileTypeFilter.Add(".jpeg");
        filePicker.FileTypeFilter.Add(".png");

        SetupBannerSettingsViewModel();
    }

    [ObservableProperty]
    Visibility timeDependentVisibility = Visibility.Visible;

    [ObservableProperty]
    Visibility galleryVisibility = Visibility.Collapsed;

    [ObservableProperty]
    Visibility customPictureVisibility = Visibility.Collapsed;

    [ObservableProperty]
    Visibility solidColorVisibility = Visibility.Collapsed;


    public void SetupBannerSettingsViewModel()
    {
        LoadTimeDependent();
        LoadGallery();
        LoadCustomPictures();

        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                SelectedTimeDependentItem = Configuration.Apperance.SelectedHomeBanner;
                break;
            case BannerType.Gallery:
                BannerBrush = new ImageBrush() { ImageSource = Uri.IsWellFormedUriString(Configuration.Apperance.HomeBannerUri, UriKind.Absolute) ? Configuration.Apperance.HomeBannerUri.AsImage(false) : "Banners/NoBanner.png".AsImage(), Stretch = Stretch.UniformToFill };
                break;
            case BannerType.CustomPicture:
                SelectedCustomPicture = Configuration.Apperance.SelectedHomeBanner;
                break;
            case BannerType.SolidColor:
                SelectedSolidColor = Configuration.Apperance.SelectedHomeBanner;
                break;
        };
        SelectedBannerType = Configuration.Apperance.HomeBanner;
    }

    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);


    [ObservableProperty]
    Brush bannerBrush = default!;

    void ResetBannerBrush()
    {
        BannerBrush = new ImageBrush() { ImageSource = "Banners/NoBanner.png".AsImage(), Stretch = Stretch.UniformToFill };
        logger.Log("Reset banner brush");
    }


    [ObservableProperty]
    BannerType selectedBannerType = default!;

    partial void OnSelectedBannerTypeChanged(BannerType value)
    {
        switch (value)
        {
            case BannerType.TimeDependent:
                TimeDependentVisibility = Visibility.Visible;
                GalleryVisibility = Visibility.Collapsed;
                CustomPictureVisibility = Visibility.Collapsed;
                SolidColorVisibility = Visibility.Collapsed;

                OnSelectedTimeDependentItemChanged(SelectedTimeDependentItem);
                break;
            case BannerType.Gallery:
                TimeDependentVisibility = Visibility.Collapsed;
                GalleryVisibility = Visibility.Visible;
                CustomPictureVisibility = Visibility.Collapsed;
                SolidColorVisibility = Visibility.Collapsed;
                break;
            case BannerType.CustomPicture:
                TimeDependentVisibility = Visibility.Collapsed;
                GalleryVisibility = Visibility.Collapsed;
                CustomPictureVisibility = Visibility.Visible;
                SolidColorVisibility = Visibility.Collapsed;

                OnSelectedCustomPictureChanged(SelectedCustomPicture);
                break;
            case BannerType.SolidColor:
                TimeDependentVisibility = Visibility.Collapsed;
                GalleryVisibility = Visibility.Collapsed;
                CustomPictureVisibility = Visibility.Collapsed;
                SolidColorVisibility = Visibility.Visible;

                OnSelectedSolidColorChanged(SelectedSolidColor);
                break;
        };

        Configuration.Apperance.HomeBanner = value;
        logger.Log("Selected BannerType changed");
    }


    void LoadTimeDependent()
    {
        TimeDependentItems.Clear();

        // API STUFF => API NOT DONE: LOCAL TIME DEPENDENT ITEMS
        TimeDependentItems.Add(new("Icy Village",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/0.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/3.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/6.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/9.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/12.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/15.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/18.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/21.png"));
        TimeDependentItems.Add(new("Showy Forest",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/0.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/3.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/6.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/9.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/12.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/15.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/18.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/21.png"));

        logger.Log("Reloaded TimeDependentItems");
    }

    public ObservableCollection<BannerTimeDependentItem> TimeDependentItems = new();

    [ObservableProperty]
    int selectedTimeDependentItem = -1;

    partial void OnSelectedTimeDependentItemChanged(int value)
    {
        Configuration.Apperance.SelectedHomeBanner = value;

        if (value < 0 || value >= TimeDependentItems.Count)
        {
            ResetBannerBrush();
            return;
        }

        BannerBrush = new ImageBrush()
        {
            ImageSource = (DateTime.Now.Hour.RoundDown(new[] { 0, 3, 6, 9, 12, 15, 18, 21 }) switch
            {
                0 => TimeDependentItems[SelectedTimeDependentItem].I_0,
                3 => TimeDependentItems[SelectedTimeDependentItem].I_3,
                6 => TimeDependentItems[SelectedTimeDependentItem].I_6,
                9 => TimeDependentItems[SelectedTimeDependentItem].I_9,
                12 => TimeDependentItems[SelectedTimeDependentItem].I_12,
                15 => TimeDependentItems[SelectedTimeDependentItem].I_15,
                18 => TimeDependentItems[SelectedTimeDependentItem].I_18,
                21 => TimeDependentItems[SelectedTimeDependentItem].I_21,
                _ => "ms-appx:///Assets/Banners/NoBanner.png"
            }).AsImage(false),
            Stretch = Stretch.UniformToFill
        };
        logger.Log("Set home banner: TimeDependent" + DateTime.Now.Hour.RoundDown(new[] { 0, 3, 6, 9, 12, 15, 18, 21 }));
    }


    void LoadGallery()
    {
        GalleryItems.Clear();

        // API STUFF => API NOT DONE: EXAMPLE BANNER GALLERY ITEMS
        GalleryItems.Add(new("Icy Village", new()
        {
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/0.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/3.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/6.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/9.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/12.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/14.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/17.png",
            "ms-appx:///Assets/Banners/TimeDependent/Icy Village/20.png",
        }));
        GalleryItems.Add(new("Snowy Forest", new()
        {
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/0.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/3.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/6.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/9.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/12.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/14.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/17.png",
            "ms-appx:///Assets/Banners/TimeDependent/Snowy Forest/20.png",
        }));
        GalleryItems.Add(new("Fortnite", new()
        {
            "https://cdn1.epicgames.com/offer/fn/21BR_KeyArt_EGS_Launcher_Blade_2560x1440_2560x1440-5335449297e75a6cc7c72ad01609b8e1",
            "https://pbs.twimg.com/media/Eaqp2csWsAEHSYx.jpg",
            "https://i0.wp.com/mentalmars.com/wp-content/uploads/2018/05/Fortnite-Wallpaper-Preview.jpg?fit=800%2C450&ssl=1",
            "https://wallpaper.dog/large/20344409.jpg"
        }));

        logger.Log("Reloaded GalleryItems");
    }

    public ObservableCollection<BannerGalleryItem> GalleryItems = new();

    public async Task OpenBannerGalleryItem(BannerGalleryItem item)
    {
        var element = new ScrollLane() { ItemsSource = item.Collection, ItemTemplate = (DataTemplate)Application.Current.Resources["BannerGalleryItemTemplate"] };

        if (await message.ShowAsync($"{item.Title} - Count: {item.Collection.Count}", element, primaryButton: "Ok") != ContentDialogResult.Primary)
            return;

        if (string.IsNullOrEmpty((string)element.SelectedItem))
        {
            ResetBannerBrush();
            return;
        }

        Configuration.Apperance.HomeBannerUri = (string)element.SelectedItem;

        BannerBrush = new ImageBrush() { ImageSource = Uri.IsWellFormedUriString((string)element.SelectedItem, UriKind.Absolute) ? ((string)element.SelectedItem).AsImage(false) : "Banners/NoBanner.png".AsImage(), Stretch = Stretch.UniformToFill };
        logger.Log("Set home banner: Gallery");
    }


    void LoadCustomPictures()
    {
        CustomPictures.Clear();

        if (fileSystem.DirectoryExists("Assets\\Banners\\Custom"))
            foreach (var img in Directory.GetFiles("Assets\\Banners\\Custom").Where(path =>
                path.EndsWith(".jpg") ||
                path.EndsWith(".jpeg") ||
                path.EndsWith(".png")))
                CustomPictures.Add(img);

        logger.Log("Reloaded CustomPictures");
    }

    public ObservableCollection<string> CustomPictures = new();

    [ObservableProperty]
    int selectedCustomPicture = -1;

    partial void OnSelectedCustomPictureChanged(int value)
    {
        Configuration.Apperance.SelectedHomeBanner = value;

        if (value < 0 || value >= CustomPictures.Count)
        {
            ResetBannerBrush();
            return;
        }

        BannerBrush = new ImageBrush() { ImageSource = Path.Combine(Computer.CurrentDirectory, CustomPictures[value]).AsImage(false), Stretch = Stretch.UniformToFill };
        logger.Log("Set home banner: CustomPicture");
    }

    readonly FileOpenPicker filePicker = new() { SuggestedStartLocation = PickerLocationId.PicturesLibrary };

    [RelayCommand(AllowConcurrentExecutions = false)]
    async Task AddCustomPicture()
    {
        var file = await filePicker.PickSingleFileAsync();

        if (file is null || string.IsNullOrWhiteSpace(file.Path))
            return;

        try
        {
            string requestedPath = Path.Combine(Computer.CurrentDirectory, "Assets\\Banners\\Custom", file.Name);

            if (fileSystem.FileExists(requestedPath) && await message.ShowAsync("Banner already exists :(", $"It looks like there is already a banner with the same file name ({file.DisplayName}). Do you want to override it?", closeButton: "No", primaryButton: "Yes") != ContentDialogResult.Primary)
                return;

            await fileSystem.CopyFileAsync(file.Path, requestedPath, true);

            LoadCustomPictures();
        }
        catch (Exception ex)
        {
            await message.ShowAsync("Something went wrong :(", $"It looks like IcyLauncher cant copy this file. Please verify that the file still exists ({file.DisplayName}).\n\nError: {ex.Message}", closeButton: "Ok");
        }

        logger.Log("Added new CustomPicture");
    }

    [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = true, IncludeCancelCommand = true)]
    async Task ResetCustomPicturesAsync(CancellationToken cancellationToken)
    {
        foreach (string file in CustomPictures)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                await fileSystem.DeleteFileAsync(file, 1000, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    await message.ShowAsync("Somethig went wrong :(", $"It looks like IcyLauncher cant delete this file ({file}).\n\nError: {ex.Message}", closeButton: "Ok");
            }
        }

        LoadCustomPictures();
        logger.Log("Reset CustomPictures");
    }

    public async Task RemoveCustomPicture(string banner)
    {
        var file = Path.Combine(Computer.CurrentDirectory, banner);

        try
        {
            await fileSystem.DeleteFileAsync(file, 100000, CancellationToken.None);
        }
        catch (Exception ex)
        {
            await message.ShowAsync("Something went wrong :(", $"It looks like IcyLauncher cant delete this file. Please verify that you have given permissions to IcyLauncher and the file is not locked.\n\nError: {ex.Message}", closeButton: "Ok");
        }

        LoadCustomPictures();
    }


    [ObservableProperty]
    int selectedSolidColor = -1;

    partial void OnSelectedSolidColorChanged(int value)
    {
        Configuration.Apperance.SelectedHomeBanner = value;

        if (value < 0 || value >= SolidColors.Container.Count)
        {
            ResetBannerBrush();
            return;
        }

        BannerBrush = SolidColors.Container[value].Color.AsSolid();
        logger.Log("Set home banner: SolidColor");
    }

    [ObservableProperty]
    Color customColorValue = Colors.White;

    [ObservableProperty]
    string customColorName = "CustomColor";

    [RelayCommand]
    void AddSolidColor(Flyout sender)
    {
        SolidColors.Container.Add(new(CustomColorValue, CustomColorName));
        sender.Hide();

        logger.Log("Added new SolidColor");
    }

    [RelayCommand]
    public void ResetSolidColors()
    {
        SolidColors.Container = new(SolidColorCollection.Default);
        SelectedSolidColor = -1;

        logger.Log("Reset SolidColors");
    }
}