using IcyLauncher.WinUI.DataTemplates;
using Microsoft.UI.Xaml.Navigation;

namespace IcyLauncher.WinUI.Views;

public sealed partial class HomeView : Page
{
    public readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();

    public HomeView() =>
        InitializeComponent();


    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        viewModel.LoadBannerImage();
        await viewModel.AskForFeedback();
    }

    private void OnProfileContainerItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count != 0)
            ProfileTemplate.UpdateProperties((GridView)sender, e.AddedItems[0], 1, new(0, 0, 0), "inBoard");

        if (e.RemovedItems.Count != 0)
            ProfileTemplate.UpdateProperties((GridView)sender, e.RemovedItems[0], 0, new(-10, 0, 0), "outBoard");
    }
}