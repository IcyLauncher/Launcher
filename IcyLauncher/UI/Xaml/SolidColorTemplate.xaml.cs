using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

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

    private void OnDeleteClick(object sender, RoutedEventArgs e) =>
            SolidColors.Container.Remove((SolidColor)((MenuFlyoutItem)sender).Tag);
}