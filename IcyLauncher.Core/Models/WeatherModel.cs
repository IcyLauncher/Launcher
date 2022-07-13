using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;

namespace IcyLauncher.Core.Models;

public partial class WeatherModel : ObservableObject
{
    [ObservableProperty]
    string city = "N/A";

    [ObservableProperty]
    string state = "N/A";

    [ObservableProperty]
    string country = "N/A";


    [ObservableProperty]
    DateTime recieved;


    [ObservableProperty]
    double degree;

    [ObservableProperty]
    BitmapImage icon = "Weather/0.png".AsImage();

    [ObservableProperty]
    string description = "N/A";
}