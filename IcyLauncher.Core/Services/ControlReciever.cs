using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.Core.Services;

public class ControlReciever
{
    readonly ILogger logger;
    readonly INavigation navigation;

    public Grid MainGrid;
    public StackPanel TitleBar;
    public Button BackButton;
    public AnimatedIcon BackButtonIcon;
    public GradientStopCollection TitleBarIconGradientStops;
    public TextBlock TitleBarTitle;
    public Grid CurrentNavigationViewItemLayoutRoot => 
        (Grid)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(navigation.GetCurrentNavigationViewItem(), 0), 0), 0);

    public ControlReciever(ILogger<ControlReciever> logger, INavigation navigation, Window shell)
    {
        this.logger = logger;
        this.navigation = navigation;

        MainGrid = (Grid)shell.Content;
        TitleBar = (StackPanel)MainGrid.Children[0];
        BackButton = (Button)TitleBar.Children[0];
        BackButtonIcon = (AnimatedIcon)((Viewbox)BackButton.Content).Child;
        TitleBarIconGradientStops = ((LinearGradientBrush)((Path)((Viewbox)TitleBar.Children[1]).Child).Fill).GradientStops;
        TitleBarTitle = (TextBlock)TitleBar.Children[2];

        this.logger.Log("Registered Control Reciever and casted all controls");
    }
}