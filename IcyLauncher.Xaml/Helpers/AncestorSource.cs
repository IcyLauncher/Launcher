using Microsoft.UI.Xaml.Media;

namespace IcyLauncher.Xaml.Helpers;

public class AncestorSource
{
    #region General
    public static readonly DependencyProperty AncestorTypeProperty = DependencyProperty.RegisterAttached(
        "AncestorType", typeof(Type), typeof(AncestorSource), new(default(Type), OnAncestorTypeChanged));
    #endregion


    #region Actions
    public static void SetAncestorType(
        FrameworkElement element,
        Type value) =>
        element.SetValue(AncestorTypeProperty, value);

    public static Type GetAncestorType(
        FrameworkElement element) =>
        (Type)element.GetValue(AncestorTypeProperty);


    static void SetDataContext(
        FrameworkElement target)
    {
        Type ancestorType = GetAncestorType(target);

        if (ancestorType is not null)
            target.DataContext = FindParent(target, ancestorType);
    }


    static object? FindParent(
        DependencyObject dependencyObject,
        Type ancestorType)
    {
        DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

        if (parent is null || ancestorType.IsAssignableFrom(parent.GetType()))
            return parent;

        return FindParent(parent, ancestorType);
    }
    #endregion


    #region Handlers
    static void OnAncestorTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs _)
    {
        FrameworkElement target = (FrameworkElement)sender;

        if (target.IsLoaded)
            SetDataContext(target);
        else
            target.Loaded += OnTargetLoaded;
    }

    static void OnTargetLoaded(object sender, RoutedEventArgs _)
    {
        FrameworkElement target = (FrameworkElement)sender;
        SetDataContext(target);

        target.Loaded -= OnTargetLoaded;
    }
    #endregion
}