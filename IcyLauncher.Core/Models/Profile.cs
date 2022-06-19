using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace IcyLauncher.Core.Models;

public class Profile
{
    public string Title { get; set; }
    public Color Color { get; set; }
    public BitmapImage Icon { get; set; }
    public string Version { get; set; }
    public string Client { get; set; }
}