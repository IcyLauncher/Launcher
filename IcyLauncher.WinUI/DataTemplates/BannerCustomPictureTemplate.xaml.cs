using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

namespace IcyLauncher.WinUI.DataTemplates;

public partial class BannerCustomPictureTemplate : ResourceDictionary
{
    public BannerCustomPictureTemplate() =>
        InitializeComponent();


    BannerSettingsViewModel viewModel = default!;


    void OnRootLayoutPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!e.GetCurrentPoint((UIElement)sender).Properties.IsRightButtonPressed)
            return;

        if (viewModel is null)
            viewModel = App.Provider.GetRequiredService<BannerSettingsViewModel>();

        FrameworkElement senderElement = (FrameworkElement)sender;
        FlyoutBase.GetAttachedFlyout(senderElement).ShowAt(senderElement);
    }

    async void OnDeleteClick(object sender, RoutedEventArgs _) =>
        await viewModel.RemoveCustomPicture((string)((MenuFlyoutItem)sender).Tag).ConfigureAwait(false);
}