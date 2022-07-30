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
    ObservableCollection<SolidColor> solidColors = default!;

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

    [ICommand(AllowConcurrentExecutions = false)]
    public async Task ResetSolidColors()
    {
        SolidColors.Clear();
        
        SolidColors.Add(new(Color.FromArgb(255, 244, 67, 54), "Red"));
        SolidColors.Add(new(Color.FromArgb(255, 233, 30, 99), "Pink"));
        SolidColors.Add(new(Color.FromArgb(255, 156, 39, 176), "Purple"));
        SolidColors.Add(new(Color.FromArgb(255, 103, 55, 183), "Deep Purple"));
        SolidColors.Add(new(Color.FromArgb(255, 63, 81, 181), "Indigo"));
        SolidColors.Add(new(Color.FromArgb(255, 33, 150, 243), "Blue"));
        SolidColors.Add(new(Color.FromArgb(255, 3, 169, 244), "Light Blue"));
        SolidColors.Add(new(Color.FromArgb(255, 0, 188, 212), "Cyan"));
        SolidColors.Add(new(Color.FromArgb(255, 0, 150, 136), "Teal"));
        SolidColors.Add(new(Color.FromArgb(255, 78, 175, 80), "Green"));
        SolidColors.Add(new(Color.FromArgb(255, 139, 195, 74), "Light Green"));
        SolidColors.Add(new(Color.FromArgb(255, 205, 220, 57), "Lime"));
        SolidColors.Add(new(Color.FromArgb(255, 255, 235, 59), "Yellow"));
        SolidColors.Add(new(Color.FromArgb(255, 255, 193, 7), "Amber"));
        SolidColors.Add(new(Color.FromArgb(255, 255, 152, 0), "Orange"));
        SolidColors.Add(new(Color.FromArgb(255, 255, 87, 34), "Deep Orange"));
        SolidColors.Add(new(Color.FromArgb(255, 121, 85, 72), "Brown"));
        SolidColors.Add(new(Color.FromArgb(255, 158, 158, 158), "Grey"));
        SolidColors.Add(new(Color.FromArgb(255, 96, 125, 139), "Blue Grey"));
        SolidColors.Add(new(Color.FromArgb(255, 255, 255, 255), "White"));
        SolidColors.Add(new(Color.FromArgb(255, 15, 15, 15), "Black"));
        SolidColors.Add(new(Color.FromArgb(255, 0, 0, 0), "Pure Black"));

        await fileSystem.SaveAsTextAsync("Assets\\Banners\\SolidColors.json", converter.ToString(SolidColors), true);
    }
}