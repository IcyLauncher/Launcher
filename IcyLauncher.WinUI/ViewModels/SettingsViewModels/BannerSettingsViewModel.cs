﻿using IcyLauncher.Xaml.Elements;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels;

public partial class BannerSettingsViewModel : ObservableObject
{
    #region Setup
    readonly ILogger<ProfilesViewModel> logger;
    readonly WindowHandler windowHandler;
    readonly IFileSystem fileSystem;
    readonly INavigation navigation;
    readonly IMessage message;

    public readonly Configuration Configuration;
    public readonly SolidColorCollection SolidColors;

    public BannerSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        WindowHandler windowHandler,
        IFileSystem fileSystem,
        INavigation navigation,
        IMessage message)
    {
        this.logger = logger;
        this.windowHandler = windowHandler;
        this.fileSystem = fileSystem;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;
        SolidColors = solidColors.Value;

        SetupViewModel();
    }

    void SetupViewModel()
    {
        windowHandler.Register(filePicker);
        filePicker.FileTypeFilter.Add(".jpg");
        filePicker.FileTypeFilter.Add(".jpeg");
        filePicker.FileTypeFilter.Add(".png");


        LoadTimeDependent();
        LoadGallery();
        LoadCustomPicture();

        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                SelectedTimeDependentPack = Configuration.Apperance.SelectedHomeBanner;
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
    #endregion


    #region Navigation
    public void SetCorrectIndex() =>
        navigation.SetCurrentIndex(5);

    [ObservableProperty]
    Visibility timeDependentVisibility = Visibility.Visible;

    [ObservableProperty]
    Visibility galleryVisibility = Visibility.Collapsed;

    [ObservableProperty]
    Visibility customPictureVisibility = Visibility.Collapsed;

    [ObservableProperty]
    Visibility solidColorVisibility = Visibility.Collapsed;


    [ObservableProperty]
    BannerType selectedBannerType = default!;

    partial void OnSelectedBannerTypeChanged(
        BannerType value)
    {
        switch (value)
        {
            case BannerType.TimeDependent:
                TimeDependentVisibility = Visibility.Visible;
                GalleryVisibility = Visibility.Collapsed;
                CustomPictureVisibility = Visibility.Collapsed;
                SolidColorVisibility = Visibility.Collapsed;

                OnSelectedTimeDependentPackChanged(SelectedTimeDependentPack);
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
    #endregion


    #region Banner Image
    [ObservableProperty]
    Brush bannerBrush = default!;

    void ResetBannerBrush()
    {
        BannerBrush = new ImageBrush() { ImageSource = "Banners/NoBanner.png".AsImage(), Stretch = Stretch.UniformToFill };
        logger.Log("Reset banner brush");
    }
    #endregion


    #region TimeDependent
    void LoadTimeDependent()
    {
        TimeDependentPacks.Clear();

        // API STUFF => API NOT DONE: LOCAL TIME DEPENDENT ITEMS
        TimeDependentPacks.Add(new("Icy Village", new()
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
        TimeDependentPacks.Add(new("Snowy Forest", new()
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
    }

    public ObservableCollection<BannerTimeDependentPack> TimeDependentPacks = new();

    [ObservableProperty]
    int selectedTimeDependentPack = -1;

    partial void OnSelectedTimeDependentPackChanged(
        int value)
    {
        Configuration.Apperance.SelectedHomeBanner = value;

        if (value < 0 || value >= TimeDependentPacks.Count)
        {
            ResetBannerBrush();
            return;
        }

        IEnumerable<int> all = TimeDependentPacks[SelectedTimeDependentPack].Collection.Select(item => item.Hour);
        int searchFor = DateTime.Now.Hour.RoundDown(all);
        var item = TimeDependentPacks[SelectedTimeDependentPack].Collection.Find(item => item.Hour == searchFor);

        BannerBrush = new ImageBrush()
        {
            ImageSource = (item?.Image ?? "ms-appx:///Assets/Banners/NoBanner.png").AsImage(false),
            Stretch = Stretch.UniformToFill
        };

        logger.Log($"Set home banner: TimeDependent [{SelectedTimeDependentPack}-{searchFor}]");
    }
    #endregion


    #region Gallery
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

    public async Task OpenBannerGalleryItem(
        BannerGalleryItem item)
    {
        ScrollLane element = new()
        {
            ItemsSource = item.Collection,
            ItemTemplate = (DataTemplate)Application.Current.Resources["BannerGalleryItemTemplate"]
        };

        if (await message.ShowAsync($"{item.Title} - Count: {item.Collection.Count}", element, closeButton: "Cancel", primaryButton: "Ok") != ContentDialogResult.Primary)
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
    #endregion


    #region Custom Picture
    void LoadCustomPicture()
    {
        CustomPictures.Clear();

        if (fileSystem.DirectoryExists("Assets\\Banners\\Custom"))
            foreach (string img in Directory.GetFiles("Assets\\Banners\\Custom").Where(path =>
                path.EndsWith(".jpg") ||
                path.EndsWith(".jpeg") ||
                path.EndsWith(".png")))
                CustomPictures.Add(img);

        logger.Log("Reloaded CustomPictures");
    }

    public ObservableCollection<string> CustomPictures = new();

    [ObservableProperty]
    int selectedCustomPicture = -1;

    partial void OnSelectedCustomPictureChanged(
        int value)
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

    [RelayCommand]
    async Task AddCustomPicture()
    {
        StorageFile file = await filePicker.PickSingleFileAsync();

        if (file is null || string.IsNullOrWhiteSpace(file.Path))
            return;

        try
        {
            string requestedPath = Path.Combine(Computer.CurrentDirectory, "Assets\\Banners\\Custom", file.Name);

            if (fileSystem.FileExists(requestedPath) && await message.ShowAsync("Banner already exists :(", $"It looks like there is already a banner with the same file name ({file.DisplayName}). Do you want to override it?", closeButton: "No", primaryButton: "Yes") != ContentDialogResult.Primary)
                return;

            await fileSystem.CopyFileAsync(file.Path, requestedPath, true);

            LoadCustomPicture();
        }
        catch (Exception ex)
        {
            await message.ShowAsync("Something went wrong :(", $"It looks like IcyLauncher cant copy this file. Please verify that the file still exists ({file.DisplayName}).\n\nError: {ex.Message}");
        }

        logger.Log("Added new CustomPicture");
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    async Task ResetCustomPicturesAsync()
    {
        foreach (string file in CustomPictures)
        {
            try
            {
                fileSystem.DeleteFile(file);
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    await message.ShowAsync("Somethig went wrong :(", $"It looks like IcyLauncher cant delete this file ({file}).\n\nError: {ex.Message}");
            }
        }

        LoadCustomPicture();
        logger.Log("Reset CustomPictures");
    }

    public async Task RemoveCustomPicture(
        string banner)
    {
        string file = Path.Combine(Computer.CurrentDirectory, banner);

        try
        {
            fileSystem.DeleteFile(file);
        }
        catch (Exception ex)
        {
            await message.ShowAsync("Something went wrong :(", $"It looks like IcyLauncher cant delete this file. Please verify that you have given permissions to IcyLauncher and the file is not locked.\n\nError: {ex.Message}");
        }

        LoadCustomPicture();
    }
    #endregion


    #region SolidColor
    [ObservableProperty]
    int selectedSolidColor = -1;

    partial void OnSelectedSolidColorChanged(
        int value)
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
    string customColorTitle = "CustomColor";

    [RelayCommand]
    void AddSolidColor(
        Flyout sender)
    {
        SolidColors.Container.Add(new(CustomColorTitle, CustomColorValue));
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
    #endregion
}