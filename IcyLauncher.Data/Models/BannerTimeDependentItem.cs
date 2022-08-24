namespace IcyLauncher.Data.Models;

public class BannerTimeDependentItem
{
    public BannerTimeDependentItem() { }

    public BannerTimeDependentItem(string title,
        string i_0,
        string i_3,
        string i_6,
        string i_9,
        string i_12,
        string i_15,
        string i_18,
        string i_21)
    {
        Title = title;
        I_0 = i_0;
        I_3 = i_3;
        I_6 = i_6;
        I_9 = i_9;
        I_12 = i_12;
        I_15 = i_15;
        I_18 = i_18;
        I_21 = i_21;
    }

    public string Title { get; set; } = "N/A";
    public string I_0 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_3 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_6 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_9 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_12 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_15 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_18 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
    public string I_21 { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";

}