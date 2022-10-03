using IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly UIElementReciever uIElementReciever;
    readonly IBackdropHandler micaBackdropHandler;
    readonly IBackdropHandler acrylicBackdropHandler;
    readonly IBackdropHandler vibrancyBackdropHandler;
    readonly BackdropHandler backdropHandler;
    readonly IConverter converter;
    readonly ImagingUtility imagingUtility;
    readonly IFileSystem fileSystem;
    readonly Updater updater;
    readonly INavigation navigation;
    readonly IMessage message;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        ConfigurationManager configurationManager,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        UIElementReciever uIElementReciever,
        MicaBackdropHandler micaBackdropHandler,
        AcrylicBackdropHandler acrylicBackdropHandler,
        VibrancyBackdropHandler vibrancyBackdropHandler,
        BackdropHandler backdropHandler,
        IConverter converter,
        ImagingUtility imagingUtility,
        IFileSystem fileSystem,
        Updater updater,
        INavigation navigation,
        IMessage message)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.uIElementReciever = uIElementReciever;
        this.micaBackdropHandler = micaBackdropHandler;
        this.acrylicBackdropHandler = acrylicBackdropHandler;
        this.vibrancyBackdropHandler = vibrancyBackdropHandler;
        this.backdropHandler = backdropHandler;
        this.converter = converter;
        this.imagingUtility = imagingUtility;
        this.fileSystem = fileSystem;
        this.updater = updater;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;


        Tabs = new()
        {
            new TabViewItem()
            {
                Header = "Home",
                IconSource = new FontIconSource() { Glyph = "\uE70F", FontFamily = new("Assets/FluentSystemIcons-Regular.ttf#FluentSystemIcons-Regular") },
                IsClosable = false,
                Content = new HomeView(new(Configuration, logger, message, ShowAddButtonFlyoutCommand))
            }
        };
    }


    public void OnPageLoaded(object sender, RoutedEventArgs _1)
    {
        navigation.SetCurrentIndex(5);


        TabView tabView = (TabView)((Page)sender).Content;

        if (flyout is null || addButton is null)
        {
            Grid mainGrid = (Grid)VisualTreeHelper.GetChild(tabView, 0);
            Grid tabGrid = (Grid)mainGrid.Children[0];
            Border buttonContainer = (Border)tabGrid.Children[3];

            addButton = (Button)buttonContainer.Child;
            flyout = FlyoutBase.GetAttachedFlyout(tabView);
        }
    }


    public ObservableCollection<TabViewItem> Tabs;

    [ObservableProperty]
    int selectedIndex;


    Button? addButton = null;
    FlyoutBase? flyout = null;

    [RelayCommand]
    void ShowAddButtonFlyout()
    {
        if (flyout is not null)
            flyout.ShowAt(addButton);
    }


    [RelayCommand]
    void AddTabViewItem(MenuFlyoutItem menuItem)
    {
        if (Tabs.Any(t => t.Header.Equals(menuItem.Text)))
        {
            SelectedIndex = Tabs.IndexOf(Tabs.First(t => t.Header.Equals(menuItem.Text)));
            return;
        }

        TabViewItem? item = menuItem.Text switch
        {
            "ILogger<T>" => new() { Content = new ILoggerView(new(logger, themeManager, windowHandler, message)) },
            "ConfigurationManager" => new() { Content = new ConfigurationManagerView(new(configurationManager, converter, message)) },
            "ThemeManager" => new() { Content = new ThemeManagerView(new(themeManager, message)) },
            "WindowHandler" => new() { Content = new WindowHandlerView(new(windowHandler, uIElementReciever, message)) },
            "UIElementReciever" => new() { Content = new UIElementRecieverView(new(uIElementReciever)) },
            "MicaBackdropHandler" => new() { Content = new MicaBackdropHandlerView(new(micaBackdropHandler, message)) },
            "AcrylicBackdropHandler" => new() { Content = new AcrylicBackdropHandlerView(new(acrylicBackdropHandler, message)) },
            "VibrancyBackdropHandler" => new() { Content = new VibrancyBackdropHandlerView(new(vibrancyBackdropHandler, message)) },
            "BackdropHandler" => new() { Content = new BackdropHandlerView(new(backdropHandler, message)) },
            "IConverter" => new() { Content = new IConverterView(new(converter, message)) },
            "ImagingUtility" => new() { Content = new ImagingUtilityView(new(imagingUtility, message)) },
            "IFileSystem" => new() { Content = new IFileSystemView(new(fileSystem, message)) },
            "Updater" => new() { Content = new UpdaterView(new(updater)) },
            "INavigation" => new() { Content = new INavigationView(new(navigation, message)) },
            "IMessage" => new() { Content = new IMessageView(new(message)) },
            _ => null
        };

        if (item is null)
            return;

        FontIcon icon = (FontIcon)menuItem.Icon;
        item.IconSource = new FontIconSource() { Glyph = icon.Glyph, FontFamily = icon.FontFamily };
        item.Header = menuItem.Text;
        ToolTipService.SetToolTip(item, ToolTipService.GetToolTip(menuItem));
        Tabs.Add(item);
        SelectedIndex = Tabs.Count - 1;
    }

    public void OnTabCloseRequested(TabView _, TabViewTabCloseRequestedEventArgs args)
    {
        Tabs.Remove(args.Tab);
    }
}