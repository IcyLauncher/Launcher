using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace IcyLauncher.Data.Models;

public class Profile
{
    public Profile() { }

    public Profile(string title, Color color, BitmapImage icon, string version, string client)
    {
        Title = title;
        Color = color;
        Icon = icon;
        Version = version;
        Client = client;
    }

    public string Title { get; set; } = "N/A";
    public Color Color { get; set; } = Colors.White;
    public BitmapImage Icon { get; set; } = "NoImage.png".AsImage();
    public string Version { get; set; } = "N/A";
    public string Client { get; set; } = "N/A";
}