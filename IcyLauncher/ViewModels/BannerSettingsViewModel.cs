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
    readonly IConverter converter;

    public readonly Configuration Configuration;

    public BannerSettingsViewModel(IOptions<Configuration> configuration,
        ILogger<ProfilesViewModel> logger,
        IFileSystem fileSystem,
        IConverter converter)
    {
        this.logger = logger;
        this.fileSystem = fileSystem;
        this.converter = converter;

        Configuration = configuration.Value;
    }


    public async void OnPageLoaded(object _, RoutedEventArgs _1)
    {
        SolidColors = converter.ToObject<ObservableCollection<SolidColor>>(await fileSystem.ReadAsTextAsync("Assets\\Banners\\SolidColors.json"));

        switch (Configuration.Apperance.HomeBanner)
        {
            case BannerType.TimeDependent:
                break;

            case BannerType.Gallery:
                break;

            case BannerType.CustomPicture:
                break;

            case BannerType.SolidColor:
                if (SolidColors.Count > Configuration.Apperance.SelectedHomeBanner)
                    SelectedSolidColor = Configuration.Apperance.SelectedHomeBanner;
                break;
        };
    }


    [ObservableProperty]
    Brush bannerBrush = default!;


    [ObservableProperty]
    ObservableCollection<SolidColor> solidColors;

    [ObservableProperty]
    int selectedSolidColor = -1;

    partial void OnSelectedSolidColorChanged(int value)
    {
        if (value is -1 || Configuration.Apperance.HomeBanner != BannerType.SolidColor)
            return;

        BannerBrush = new SolidColorBrush(SolidColors[value].Color);
        Configuration.Apperance.SelectedHomeBanner = value;

        logger.Log("Updated home banner: SolidColor");
    }

    [ObservableProperty]
    Color customColorValue = Colors.White;

    [ObservableProperty]
    string customColorName = "CustomColor";

    [ICommand(AllowConcurrentExecutions = false)]
    async Task AddCustomColor(Flyout sender)
    {
        SolidColors.Add(new(CustomColorValue, CustomColorName));
        sender.Hide();

        await fileSystem.SaveAsTextAsync("Assets\\Banners\\SolidColors.json", converter.ToString(SolidColors), true);
    }

    public async Task DeleteSolidColor(SolidColor solCol)
    {
        SolidColors.Remove(solCol);
        await fileSystem.SaveAsTextAsync("Assets\\Banners\\SolidColors.json", converter.ToString(SolidColors), true);
    }
}