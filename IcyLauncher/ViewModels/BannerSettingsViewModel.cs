using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class BannerSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly WindowHandler windowHandler;
    readonly IFileSystem fileSystem;
    readonly IMessage message;

    public readonly Configuration Configuration;
    public readonly SolidColorCollection SolidColors;

    public BannerSettingsViewModel(
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ILogger<ProfilesViewModel> logger,
        WindowHandler windowHandler,
        IFileSystem fileSystem,
        IMessage message)
    {
        this.logger = logger;
        this.windowHandler = windowHandler;
        this.fileSystem = fileSystem;
        this.message = message;

        Configuration = configuration.Value;
        SolidColors = solidColors.Value;


        LoadCustomPictures();

        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                break;
            case BannerType.Gallery:
                break;
            case BannerType.CustomPicture:
                SelectedCustomPicture = Configuration.Apperance.SelectedHomeBanner;
                break;
            case BannerType.SolidColor:
                SelectedSolidColor = Configuration.Apperance.SelectedHomeBanner;
                break;
        };
        SelectedBannerType = Configuration.Apperance.HomeBanner;

        this.windowHandler.Register(filePicker);
        filePicker.FileTypeFilter.Add(".jpg");
        filePicker.FileTypeFilter.Add(".jpeg");
        filePicker.FileTypeFilter.Add(".png");
    }


    [ObservableProperty]
    Brush bannerBrush = default!;

    void ResetBannerBrush()
    {
        BannerBrush = new ImageBrush() { ImageSource = "Banners/NoBanner.png".AsImage(), Stretch = Stretch.UniformToFill };
        logger.Log("Reset banner brush: NoBanner.png");
    }


    [ObservableProperty]
    BannerType selectedBannerType = default!;

    partial void OnSelectedBannerTypeChanged(BannerType value)
    {
        switch (value)
        {
            case BannerType.TimeDependent:
                break;
            case BannerType.Gallery:
                break;
            case BannerType.CustomPicture:
                OnSelectedCustomPictureChanged(SelectedCustomPicture);
                break;
            case BannerType.SolidColor:
                OnSelectedSolidColorChanged(SelectedSolidColor);
                break;
        };

        Configuration.Apperance.HomeBanner = value;
        logger.Log("Selected banner type changed");
    }


    void LoadCustomPictures()
    {
        CustomPictures.Clear();
        foreach (var img in Directory.GetFiles("Assets\\Banners\\Custom").Where(path =>
            path.EndsWith(".jpg") ||
            path.EndsWith(".jpeg") ||
            path.EndsWith(".png")))
            CustomPictures.Add(img);

        logger.Log("Reloaded custom pictures");
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

    [ICommand(AllowConcurrentExecutions = false)]
    async Task AddCustomPicture()
    {
        var file = await filePicker.PickSingleFileAsync();

        if (file is null || string.IsNullOrWhiteSpace(file.Path))
            return;

        try
        {
            string requestedPath = Path.Combine(Computer.CurrentDirectory, "Assets\\Banners\\Custom", file.Name);

            if (fileSystem.FileExists(requestedPath) && await message.ShowAsync("Banner already exists :(", $"It looks like there is already a banner with the same file name ({file.DisplayName}). Do you want to override it?", true, "No", "Yes") != ContentDialogResult.Primary)
                return;

            fileSystem.CopyFile(file.Path, requestedPath, true);

            LoadCustomPictures();
        }
        catch (Exception ex)
        {
            await message.ShowAsync("Something went wrong :(", $"It looks like IcyLauncher cant copy this file. Please verify that the file still exists ({file.DisplayName}).\n\nError: {ex.Message}", true, "Ok");
        }

        logger.Log("Added custom picture");
    }

    [ICommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
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
                    await message.ShowAsync("Somethig went wrong :(", $"It looks like IcyLauncher cant delete this file ({file}).\n\nError: {ex.Message}", true, "Ok");
            }
        }

        LoadCustomPictures();
        logger.Log("Reset custom pictures");
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
            await message.ShowAsync("Something went wrong :(", $"It looks like IcyLauncher cant delete this file. Please verify that you have given permissions to IcyLauncher and the file is not locked.\n\nError: {ex.Message}", true, "Ok");
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

        BannerBrush = new SolidColorBrush(SolidColors.Container[value].Color);
        logger.Log("Set home banner: SolidColor");
    }

    [ObservableProperty]
    Color customColorValue = Colors.White;

    [ObservableProperty]
    string customColorName = "CustomColor";

    [ICommand]
    void AddSolidColor(Flyout sender)
    {
        SolidColors.Container.Add(new(CustomColorValue, CustomColorName));
        sender.Hide();

        logger.Log("Added custom solid color");
    }

    [ICommand]
    public void ResetSolidColors()
    {
        SolidColors.Container = new(SolidColorCollection.Default);
        SelectedSolidColor = -1;

        logger.Log("Reset solid color");
    }
}