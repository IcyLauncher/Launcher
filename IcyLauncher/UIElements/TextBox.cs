using IcyLauncher.Core.Xaml;

namespace IcyLauncher.UIElements;

public class TextBox : Microsoft.UI.Xaml.Controls.TextBox
{
    public TextBox()
    {
        Loaded += OnLoaded;
        IsEnabledChanged += UpdateBinding;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        UpdateBinding(sender, null);
    }

    private void UpdateBinding(object sender, object? args) =>
        SetBinding(ForegroundProperty, new Microsoft.UI.Xaml.Data.Binding()
        {
            Source = App.Colors,
            Converter = new ValidateNo255(),
            Path = new PropertyPath(IsEnabled ? "Text.Secondary" : "Text.Disabled"),
            Mode = Microsoft.UI.Xaml.Data.BindingMode.OneWay
        });
}