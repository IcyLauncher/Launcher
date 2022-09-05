using Windows.UI;

namespace IcyLauncher.Data.Models;

public partial class ButtonColors
{
    public ButtonColors(
        Color normal,
        Color hover,
        Color pressed,
        Color inactive)
    {
        Normal = normal;
        Hover = hover;
        Pressed = pressed;
        Inactive = inactive;
    }

    public Color Normal;
    public Color Hover;
    public Color Pressed;
    public Color Inactive;
}