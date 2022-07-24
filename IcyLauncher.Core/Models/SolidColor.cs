using Windows.UI;

namespace IcyLauncher.Core.Models;

public class SolidColor
{
    public SolidColor()
    {
    }

    public SolidColor(Color color, string name)
    {
        Color = color;
        Name = name;
    }

    public Color Color { get; set; } = default!;
    public string Name { get; set; } = default!;
}