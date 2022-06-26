using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Numerics;

namespace IcyLauncher.UI;

public partial class ProfileDataTemplate : ResourceDictionary
{
    public ProfileDataTemplate() =>
        InitializeComponent();

    private void OnRootLayoutLoaded(object sender, RoutedEventArgs e)
    {
        var rootLayout = (Grid)sender;
        var details = rootLayout.Children[3];
        var icon = rootLayout.Children[2];
        var item = (GridViewItem)VisualTreeHelper.GetParent((Grid)((ContentPresenter)VisualTreeHelper.GetParent(rootLayout)).Parent);

        rootLayout.PointerEntered += (s, e) =>
        {
            if (!item.IsSelected)
            {
                details.Opacity = 1;
                details.Translation = new Vector3(0, 0, 0);
                icon.Translation = new Vector3(40, 0, 0);
            };
        };
        rootLayout.PointerExited += (s, e) =>
        {
            if (!item.IsSelected)
            {
                details.Opacity = 0;
                details.Translation = new Vector3(-10, 0, 0);
                icon.Translation = new Vector3(0, 0, 0);
            }
        };

        rootLayout.Loaded -= OnRootLayoutLoaded;
    }
}