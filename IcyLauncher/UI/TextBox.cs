using IcyLauncher.Core.Xaml;
using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.UI;

public class TextBox : Microsoft.UI.Xaml.Controls.TextBox
{
    Theme colors = default!;

    public TextBox()
    {
        Loaded += OnLoaded;
        IsEnabledChanged += UpdateBinding;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        colors = App.Provider.GetRequiredService<ThemeManager>().Colors;
        UpdateBinding(sender, null);

        Loaded -= OnLoaded;
    }

    private void UpdateBinding(object sender, object? args) =>
        SetBinding(ForegroundProperty, new Binding()
        {
            Source = colors,
            Converter = UIElementProvider.ValidateNo255,
            Path = new PropertyPath(IsEnabled ? "Text.Secondary" : "Text.Disabled"),
            Mode = BindingMode.OneWay
        });
}