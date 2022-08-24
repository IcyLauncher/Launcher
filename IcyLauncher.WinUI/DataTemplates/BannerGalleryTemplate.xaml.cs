using IcyLauncher.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.WinUI.DataTemplates;

public partial class BannerGalleryTemplate : ResourceDictionary
{
    public BannerGalleryTemplate() =>
        InitializeComponent();


    BannerSettingsViewModel viewModel = default!;


    private void OnRootLayoutLoaded(object sender, RoutedEventArgs e)
    {
        var layoutRoot = (Grid)sender;
        var gallery = (BannerGalleryItem)layoutRoot.DataContext;

        if (0 >= gallery.Collection.Count)
            return;
        ((Rectangle)layoutRoot.Children[0]).Fill = new ImageBrush() { ImageSource = gallery.Collection[0].AsImage(false), Stretch = Stretch.UniformToFill };

        if (1 >= gallery.Collection.Count)
            return;
        ((Rectangle)layoutRoot.Children[1]).Fill = new ImageBrush() { ImageSource = gallery.Collection[1].AsImage(false), Stretch = Stretch.UniformToFill };

        if (2 >= gallery.Collection.Count)
            return;
        ((Rectangle)layoutRoot.Children[2]).Fill = new ImageBrush() { ImageSource = gallery.Collection[2].AsImage(false), Stretch = Stretch.UniformToFill };

        if (3 >= gallery.Collection.Count)
            return;
        ((Rectangle)layoutRoot.Children[3]).Fill = new ImageBrush() { ImageSource = gallery.Collection[3].AsImage(false), Stretch = Stretch.UniformToFill };
    }

    private async void OnOpenItemClicked(object sender, RoutedEventArgs e)
    {
        if (viewModel is null)
            viewModel = App.Provider.GetRequiredService<BannerSettingsViewModel>();

        await viewModel.OpenBannerGalleryItem((BannerGalleryItem)((Grid)((Button)sender).Parent).DataContext);
    }
}