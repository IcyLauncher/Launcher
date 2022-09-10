namespace IcyLauncher.Xaml.Elements;

public class HCont : ContentControl
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title", typeof(string), typeof(HCont), new("Card Title"));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }


    public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
        "ContentHeight", typeof(double), typeof(HCont), new(double.NaN));

    public double ContentHeight
    {
        get => (double)GetValue(ContentHeightProperty);
        set => SetValue(ContentHeightProperty, value);
    }


    public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register(
        "ContentWidth", typeof(double), typeof(HCont), new(double.NaN));

    public double ContentWidth
    {
        get => (double)GetValue(ContentWidthProperty);
        set => SetValue(ContentWidthProperty, value);
    }


    public static readonly DependencyProperty VerticalScrollBarProperty = DependencyProperty.Register(
        "VerticalScrollBar", typeof(ScrollBarVisibility), typeof(HCont), new(ScrollBarVisibility.Disabled));

    public ScrollBarVisibility VerticalScrollBar
    {
        get => (ScrollBarVisibility)GetValue(VerticalScrollBarProperty);
        set => SetValue(VerticalScrollBarProperty, value);
    }


    public static readonly DependencyProperty HorizontalScrollBarProperty = DependencyProperty.Register(
        "HorizontalScrollBar", typeof(ScrollBarVisibility), typeof(HCont), new(ScrollBarVisibility.Disabled));

    public ScrollBarVisibility HorizontalScrollBar
    {
        get => (ScrollBarVisibility)GetValue(HorizontalScrollBarProperty);
        set => SetValue(HorizontalScrollBarProperty, value);
    }


    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        TextBlock header = (TextBlock)GetTemplateChild("Header");
        ScrollViewer scrollContainer = (ScrollViewer)GetTemplateChild("ScrollContainer");

        scrollContainer.ViewChanged += (s, e) =>
            header.Opacity = scrollContainer.VerticalOffset <= 5 ? 1 : 0;
    }
}