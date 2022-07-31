using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class BannerSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly IFileSystem fileSystem;

    public readonly Configuration Configuration;
    public readonly SolidColorCollection SolidColors;

    public BannerSettingsViewModel(
        IOptions<Configuration> configuration,
        IOptions<SolidColorCollection> solidColors,
        ILogger<ProfilesViewModel> logger,
        IFileSystem fileSystem)
    {
        this.logger = logger;
        this.fileSystem = fileSystem;

        Configuration = configuration.Value;
        SolidColors = solidColors.Value;
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1)
    {
        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                break;
            case BannerType.Gallery:
                break;
            case BannerType.CustomPicture:
                break;
            case BannerType.SolidColor:
                if (SolidColors.Container.Count > Configuration.Apperance.SelectedHomeBanner)
                    SelectedSolidColor = Configuration.Apperance.SelectedHomeBanner;
                break;
        };
    }


    [ObservableProperty]
    Brush bannerBrush = default!;


    [ObservableProperty]
    int selectedSolidColor = -1;

    partial void OnSelectedSolidColorChanged(int value)
    {
        if (value is -1 || Configuration.Apperance.HomeBanner != BannerType.SolidColor)
            return;

        BannerBrush = new SolidColorBrush(SolidColors.Container[value].Color);
        Configuration.Apperance.SelectedHomeBanner = value;

        logger.Log("Updated home banner: SolidColor");
    }

    [ObservableProperty]
    Color customColorValue = Colors.White;

    [ObservableProperty]
    string customColorName = "CustomColor";

    [ICommand]
    void AddCustomColor(Flyout sender)
    {
        SolidColors.Container.Add(new(CustomColorValue, CustomColorName));
        sender.Hide();
    }

    [ICommand]
    public void ResetSolidColors()
    {
        SolidColors.Container = new(SolidColorCollection.Default);
        SelectedSolidColor = 0;
    }
}