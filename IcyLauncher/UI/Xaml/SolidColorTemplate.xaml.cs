using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.UI.Xaml;

public partial class SolidColorTemplate : ResourceDictionary
{
    public SolidColorTemplate() =>
        InitializeComponent();


    public readonly SolidColorCollection SolidColors = App.Provider.GetRequiredService<IOptions<SolidColorCollection>>().Value;


    private void OnRootLayoutPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!e.GetCurrentPoint((UIElement)sender).Properties.IsRightButtonPressed)
            return;

        var senderElement = (FrameworkElement)sender;
        FlyoutBase.GetAttachedFlyout(senderElement).ShowAt(senderElement);
    }

    private void OnRootLayoutLoaded(object sender, RoutedEventArgs e)
    {
        var rootLayout = (Grid)sender;
        var rect = (Rectangle)rootLayout.Children[0];
        var name = (TextBlock)rootLayout.Children[1];

        var ele = SolidColors.Container.Where(solCol => solCol.Color == ((SolidColorBrush)rect.Fill).Color && solCol.Name == name.Text).ToArray();
        if (ele.Length == 0)
            return;

        var senderElement = (FrameworkElement)sender;
        var flyout = (MenuFlyout)FlyoutBase.GetAttachedFlyout(senderElement);

        ((MenuFlyoutItem)flyout.Items[0]).Click += (s, e) =>
            SolidColors.Container.Remove(ele[0]);
    }
}