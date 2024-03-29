﻿using IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;
using IcyLauncher.Xaml.UI;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    #region Setup
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly IBackdropHandler micaBackdropHandler;
    readonly IBackdropHandler acrylicBackdropHandler;
    readonly IBackdropHandler vibrancyBackdropHandler;
    readonly BackdropHandler backdropHandler;
    readonly IConverter converter;
    readonly ImagingUtility imagingUtility;
    readonly FeedbackRequest feedbackRequest;
    readonly IFileSystem fileSystem;
    readonly Updater updater;
    readonly INavigation navigation;
    readonly IMessage message;
    readonly CoreWindow shell;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        ConfigurationManager configurationManager,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        MicaBackdropHandler micaBackdropHandler,
        AcrylicBackdropHandler acrylicBackdropHandler,
        VibrancyBackdropHandler vibrancyBackdropHandler,
        BackdropHandler backdropHandler,
        IConverter converter,
        ImagingUtility imagingUtility,
        FeedbackRequest feedbackRequest,
        IFileSystem fileSystem,
        Updater updater,
        INavigation navigation,
        IMessage message,
        CoreWindow shell)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.micaBackdropHandler = micaBackdropHandler;
        this.acrylicBackdropHandler = acrylicBackdropHandler;
        this.vibrancyBackdropHandler = vibrancyBackdropHandler;
        this.backdropHandler = backdropHandler;
        this.converter = converter;
        this.imagingUtility = imagingUtility;
        this.feedbackRequest = feedbackRequest;
        this.fileSystem = fileSystem;
        this.updater = updater;
        this.navigation = navigation;
        this.message = message;
        this.shell = shell;

        Configuration = configuration.Value;

        SetupViewModel();
    }

    void SetupViewModel()
    {
        Tabs = new()
        {
            new TabViewItem()
            {
                Header = "Home",
                IconSource = new FontIconSource() { Glyph = "\uE70F", FontFamily = (FontFamily)Application.Current.Resources["FluentRegular"] },
                IsClosable = false,
                Content = new HomeView(new(Configuration, logger, message, ShowAddButtonFlyoutCommand))
            }
        };
    }

    [RelayCommand]
    void SetupPage(TabView tabContainer)
    {
        navigation.SetCurrentIndex(5);


        if (flyout is null || addButton is null)
        {
            Grid mainGrid = (Grid)VisualTreeHelper.GetChild(tabContainer, 0);
            Grid tabGrid = (Grid)mainGrid.Children[0];
            Border buttonContainer = (Border)tabGrid.Children[3];

            addButton = (Button)buttonContainer.Child;
            flyout = FlyoutBase.GetAttachedFlyout(tabContainer);
        }
    }
    #endregion


    #region Flyout
    Button? addButton;
    FlyoutBase? flyout;

    [RelayCommand]
    void ShowAddButtonFlyout()
    {
        if (flyout is not null)
            flyout.ShowAt(addButton);
    }
    #endregion


    #region TabContainer
    public ObservableCollection<TabViewItem> Tabs = default!;

    [ObservableProperty]
    int selectedIndex;


    [RelayCommand]
    void AddTabViewItem(
        MenuFlyoutItem menuItem)
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
            "WindowHandler" => new() { Content = new WindowHandlerView(new(windowHandler, message, shell)) },
            "MicaBackdropHandler" => new() { Content = new MicaBackdropHandlerView(new(micaBackdropHandler, message)) },
            "AcrylicBackdropHandler" => new() { Content = new AcrylicBackdropHandlerView(new(acrylicBackdropHandler, message)) },
            "VibrancyBackdropHandler" => new() { Content = new VibrancyBackdropHandlerView(new(vibrancyBackdropHandler, message)) },
            "BackdropHandler" => new() { Content = new BackdropHandlerView(new(backdropHandler, message)) },
            "IConverter" => new() { Content = new IConverterView(new(converter, message)) },
            "ImagingUtility" => new() { Content = new ImagingUtilityView(new(imagingUtility, message)) },
            "FeedbackRequest" => new() { Content = new FeedbackRequestView(new(feedbackRequest, message)) },
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

    [RelayCommand]
    void RemoveTabViewItem(
        TabViewTabCloseRequestedEventArgs args) =>
        Tabs.Remove(args.Tab);
    #endregion
}