using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.UI;

public class ScrollLane : GridView
{
    public static readonly DependencyProperty CanContentRenderOutsideBoundsProperty = DependencyProperty.Register(
        "CanContentRenderOutsideBounds",
        typeof(bool),
        typeof(ScrollLane),
        new PropertyMetadata(true));

    public bool CanContentRenderOutsideBounds
    {
        get => (bool)GetValue(CanContentRenderOutsideBoundsProperty);
        set => SetValue(CanContentRenderOutsideBoundsProperty, value);
    }

    public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register(
        "VerticalScrollBarVisibility",
        typeof(ScrollBarVisibility),
        typeof(ScrollLane),
        new PropertyMetadata(ScrollBarVisibility.Hidden));

    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
        set => SetValue(VerticalScrollBarVisibilityProperty, value);
    }

    public event SelectionChangedEventHandler? ItemSelectionChanged;


    ScrollViewer scrollContainer = default!;
    GridView itemsContainter = default!;
    Button backButton = default!;
    Button forwardButton = default!;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        scrollContainer = (ScrollViewer)GetTemplateChild("ScrollContainer");
        itemsContainter = (GridView)GetTemplateChild("ItemsContainter");
        backButton = (Button)GetTemplateChild("BackButton");
        forwardButton = (Button)GetTemplateChild("ForwardButton");

        scrollContainer.Loaded += OnScrollContainerLoaded;
        itemsContainter.Items.VectorChanged += async (s, e) =>
        {
            await Task.Delay(100);
            UpdateButtonVisibilities(scrollContainer.HorizontalOffset);
        };
        itemsContainter.SelectionChanged += ItemSelectionChanged;
        backButton.Click += (s, e) =>
        {
            var newScroll = scrollContainer.HorizontalOffset - scrollContainer.ActualWidth + 50;

            if (scrollContainer.ChangeView(newScroll, null, null))
                UpdateButtonVisibilities(newScroll);
        };
        forwardButton.Click += (s, e) =>
        {
            var newScroll = scrollContainer.HorizontalOffset + scrollContainer.ActualWidth - 50;

            if (scrollContainer.ChangeView(newScroll, null, null))
                UpdateButtonVisibilities(newScroll);
        };

        UpdateButtonVisibilities(scrollContainer.HorizontalOffset);
    }

    private void OnScrollContainerLoaded(object sender, RoutedEventArgs e)
    {
        scrollContainer.SizeChanged += (s, e) => UpdateButtonVisibilities(scrollContainer.HorizontalOffset);
        scrollContainer.Loaded -= OnScrollContainerLoaded;
    }


    private void UpdateButtonVisibilities(double newScroll)
    {
        backButton.Visibility = newScroll > 0 && scrollContainer.ScrollableWidth > 0 ? Visibility.Visible : Visibility.Collapsed;
        forwardButton.Visibility = newScroll < scrollContainer.ScrollableWidth && scrollContainer.ScrollableWidth > 0 ? Visibility.Visible : Visibility.Collapsed;
    }
}