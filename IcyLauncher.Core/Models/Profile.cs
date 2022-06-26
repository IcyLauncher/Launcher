using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace IcyLauncher.Core.Models;

public class Profile
{
    public string Title { get; set; } = "N/A";
    public Color Color { get; set; } = Colors.White;
    public BitmapImage Icon { get; set; } = "NoImage.png".AsImage();
    public string Version { get; set; } = "N/A";
    public string Client { get; set; } = "Vanilla";
}