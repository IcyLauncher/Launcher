using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Windows.UI;

namespace IcyLauncher.Data.Models;

public class SolidColor
{
    public SolidColor() { }

    public SolidColor(
        string title,
        Color color)
    {
        Title = title;
        Color = color;
    }

    public string Title { get; set; } = "N/A";
    public Color Color { get; set; } = default!;
}

public partial class SolidColorCollection : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<SolidColor> container = default!;

    public static readonly SolidColor[] Default = new SolidColor[]
        {
            new ("Red", Color.FromArgb(255, 244, 67, 54)),
            new ("Pink", Color.FromArgb(255, 233, 30, 99)),
            new ("Purple", Color.FromArgb(255, 156, 39, 176)),
            new ("Deep Purple", Color.FromArgb(255, 103, 55, 183)),
            new ("Indigo", Color.FromArgb(255, 63, 81, 181)),
            new ("Blue", Color.FromArgb(255, 33, 150, 243)),
            new ("Light Blue", Color.FromArgb(255, 3, 169, 244)),
            new ("Cyan", Color.FromArgb(255, 0, 188, 212)),
            new ("Teal", Color.FromArgb(255, 0, 150, 136)),
            new ("Green", Color.FromArgb(255, 78, 175, 80)),
            new ("Light Green", Color.FromArgb(255, 139, 195, 74)),
            new ("Lime", Color.FromArgb(255, 205, 220, 57)),
            new ("Yellow", Color.FromArgb(255, 255, 235, 59)),
            new ("Amber", Color.FromArgb(255, 255, 193, 7)),
            new ("Orange", Color.FromArgb(255, 255, 152, 0)),
            new ("Deep Orange", Color.FromArgb(255, 255, 87, 34)),
            new ("Brown", Color.FromArgb(255, 121, 85, 72)),
            new ("Grey", Color.FromArgb(255, 158, 158, 158)),
            new ("Blue Grey", Color.FromArgb(255, 96, 125, 139)),
            new ("White", Color.FromArgb(255, 255, 255, 255)),
            new ("Black", Color.FromArgb(255, 15, 15, 15)),
            new ("Pure Black", Color.FromArgb(255, 0, 0, 0))
        };
}