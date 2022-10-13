using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

namespace IcyLauncher.WinUI.DataTemplates;

public partial class SolidColorTemplate : ResourceDictionary
{
    #region Setup
    public SolidColorTemplate() =>
        InitializeComponent();


    public readonly SolidColorCollection SolidColors = App.Provider.GetRequiredService<IOptions<SolidColorCollection>>().Value;
    #endregion


    #region Handlers
    void OnRootLayoutPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!e.GetCurrentPoint((UIElement)sender).Properties.IsRightButtonPressed)
            return;

        FrameworkElement senderElement = (FrameworkElement)sender;
        FlyoutBase.GetAttachedFlyout(senderElement).ShowAt(senderElement);
    }

    void OnDeleteClick(object sender, RoutedEventArgs _) =>
        SolidColors.Container.Remove((SolidColor)((MenuFlyoutItem)sender).Tag);
    #endregion
}