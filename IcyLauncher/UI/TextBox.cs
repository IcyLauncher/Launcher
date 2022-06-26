using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.UI;

public class TextBox : Microsoft.UI.Xaml.Controls.TextBox
{
    Theme colors = default!;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        colors = App.Provider.GetRequiredService<ThemeManager>().Colors;
        UpdateBinding();
    }

    private void UpdateBinding() =>
        SetBinding(ForegroundProperty, new Binding()
        {
            Source = colors,
            Converter = UIElementProvider.ValidateNo255,
            Path = new PropertyPath(IsEnabled ? "Text.Secondary" : "Text.Disabled"),
            Mode = BindingMode.OneWay
        });
}