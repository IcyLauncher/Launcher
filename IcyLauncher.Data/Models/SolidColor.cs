using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Windows.UI;

namespace IcyLauncher.Data.Models;

public class SolidColor
{
    public SolidColor() { }

    public SolidColor(
        Color color,
        string name)
    {
        Color = color;
        Name = name;
    }

    public Color Color { get; set; } = default!;
    public string Name { get; set; } = default!;
}

public partial class SolidColorCollection : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<SolidColor> container = default!;

    public static readonly SolidColor[] Default = new SolidColor[]
        {
            new (Color.FromArgb(255, 244, 67, 54), "Red"),
            new (Color.FromArgb(255, 233, 30, 99), "Pink"),
            new (Color.FromArgb(255, 156, 39, 176), "Purple"),
            new (Color.FromArgb(255, 103, 55, 183), "Deep Purple"),
            new (Color.FromArgb(255, 63, 81, 181), "Indigo"),
            new (Color.FromArgb(255, 33, 150, 243), "Blue"),
            new (Color.FromArgb(255, 3, 169, 244), "Light Blue"),
            new (Color.FromArgb(255, 0, 188, 212), "Cyan"),
            new (Color.FromArgb(255, 0, 150, 136), "Teal"),
            new (Color.FromArgb(255, 78, 175, 80), "Green"),
            new (Color.FromArgb(255, 139, 195, 74), "Light Green"),
            new (Color.FromArgb(255, 205, 220, 57), "Lime"),
            new (Color.FromArgb(255, 255, 235, 59), "Yellow"),
            new (Color.FromArgb(255, 255, 193, 7), "Amber"),
            new (Color.FromArgb(255, 255, 152, 0), "Orange"),
            new (Color.FromArgb(255, 255, 87, 34), "Deep Orange"),
            new (Color.FromArgb(255, 121, 85, 72), "Brown"),
            new (Color.FromArgb(255, 158, 158, 158), "Grey"),
            new (Color.FromArgb(255, 96, 125, 139), "Blue Grey"),
            new (Color.FromArgb(255, 255, 255, 255), "White"),
            new (Color.FromArgb(255, 15, 15, 15), "Black"),
            new (Color.FromArgb(255, 0, 0, 0), "Pure Black")
        };
}