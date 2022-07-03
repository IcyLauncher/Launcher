using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Collections.ObjectModel;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;

    public HomeViewModel(ThemeManager themeManager, WindowHandler windowHandler)
    {
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
    }

    static Color GetRandomColor(byte transparency = 255)
    {
        var random = new Random();
        return Color.FromArgb(transparency, Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));
    }


    [ObservableProperty]
    BlurEffect selectedBlurEffect = BlurEffect.Mica;

    partial void OnSelectedBlurEffectChanged(BlurEffect value)
    {
        windowHandler.SetBlur(value, true, darkMode);
    }

    [ObservableProperty]
    IEnumerable<BlurEffect> blurEffects = Enum.GetValues(typeof(BlurEffect)).Cast<BlurEffect>();


    [ObservableProperty]
    Color accentPrimary = GetRandomColor(255);

    [ObservableProperty]
    Color accentLight = GetRandomColor(255);

    [ObservableProperty]
    Color accentDark = GetRandomColor(255);

    [ICommand]
    void UpdateAccent()
    {
        themeManager.Colors.Accent.Primary = accentPrimary;
        themeManager.Colors.Accent.Light = accentLight;
        themeManager.Colors.Accent.Dark = accentDark;
    }

    [ObservableProperty]
    bool darkMode = true;

    partial void OnDarkModeChanged(bool value)
    {
        themeManager.LoadTheme(value ? Theme.Dark : Theme.Light, true);
        windowHandler.SetBlur(selectedBlurEffect, true, darkMode);
    }


    [ObservableProperty]
    ObservableCollection<Profile> profiles = new()
    {
        new(),
        new()
        {
            Title = "Beta",
            Icon = "Redstone-Block.png".AsImage(),
            Color = Colors.Red
        }
    };

    public void OnItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.RemovedItems.Count == 0)
            return;

        var rootLayout = (Grid)((GridViewItem)((GridView)sender).ContainerFromItem(e.RemovedItems[0])).ContentTemplateRoot;
        var details = rootLayout.Children[4];
        var icon = (Image)rootLayout.Children[2];

        details.Opacity = 0;
        details.Translation = new(-10, 0, 0);
        ((Storyboard)icon.Resources["outBoard"]).Begin();
    }
}