using Microsoft.UI.Xaml.Media;

namespace IcyLauncher.Xaml.Helpers;

public class AncestorSource
{
    public static readonly DependencyProperty AncestorTypeProperty =
        DependencyProperty.RegisterAttached(
            "AncestorType",
            typeof(Type),
            typeof(AncestorSource),
            new(default(Type), OnAncestorTypeChanged));

    public static void SetAncestorType(FrameworkElement element, Type value) =>
        element.SetValue(AncestorTypeProperty, value);

    public static Type GetAncestorType(FrameworkElement element) =>
        (Type)element.GetValue(AncestorTypeProperty);

    private static void OnAncestorTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        FrameworkElement target = (FrameworkElement)d;

        if (target.IsLoaded)
            SetDataContext(target);
        else
            target.Loaded += OnTargetLoaded;
    }

    private static void OnTargetLoaded(object sender, RoutedEventArgs e)
    {
        FrameworkElement target = (FrameworkElement)sender;
        SetDataContext(target);

        target.Loaded -= OnTargetLoaded;
    }

    private static void SetDataContext(FrameworkElement target)
    {
        Type ancestorType = GetAncestorType(target);

        if (ancestorType is not null)
            target.DataContext = FindParent(target, ancestorType);
    }

    private static object? FindParent(DependencyObject dependencyObject, Type ancestorType)
    {
        DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

        if (parent is null || ancestorType.IsAssignableFrom(parent.GetType()))
            return parent;

        return FindParent(parent, ancestorType);
    }
}