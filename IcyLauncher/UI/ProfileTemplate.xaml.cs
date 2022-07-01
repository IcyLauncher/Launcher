using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System.Numerics;

namespace IcyLauncher.UI;

public partial class ProfileTemplate : ResourceDictionary
{
    public ProfileTemplate() =>
        InitializeComponent();

    private void OnRootLayoutLoaded(object sender, RoutedEventArgs e)
    {
        var rootLayout = (Grid)sender;
        var details = rootLayout.Children[4];
        var icon = (Image)rootLayout.Children[2];
        var item = (GridViewItem)VisualTreeHelper.GetParent((Grid)((ContentPresenter)VisualTreeHelper.GetParent(rootLayout)).Parent);

        var inBoard = (Storyboard)icon.Resources["inBoard"];
        var outBoard = (Storyboard)icon.Resources["outBoard"];

        rootLayout.PointerEntered += (s, e) =>
        {
            if (!item.IsSelected)
            {
                details.Opacity = 1;
                details.Translation = new Vector3(0, 0, 0);
                inBoard.Begin();
            };
        };
        rootLayout.PointerExited += (s, e) =>
        {
            if (!item.IsSelected)
            {
                details.Opacity = 0;
                details.Translation = new Vector3(-10, 0, 0);
                outBoard.Begin();
            }
        };

        rootLayout.Loaded -= OnRootLayoutLoaded;
    }
}