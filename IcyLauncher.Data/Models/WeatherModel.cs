using Microsoft.UI.Xaml.Media.Imaging;

namespace IcyLauncher.Data.Models;

public class WeatherModel
{
    public string City = "N/A";
    public string State = "N/A";
    public string Country = "N/A";

    public DateTime Recieved;

    public double Degree;
    public string Description = "N/A";
    public BitmapImage Icon = "Weather/0.png".AsImage();
}