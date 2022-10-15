using System.Collections.Generic;

namespace IcyLauncher.Data.Models;

public class BannerTimeDependentPack
{
    public BannerTimeDependentPack() { }

    public BannerTimeDependentPack(
        string title,
        List<BannerTimeDependentItem> collection)
    {
        Title = title;
        Collection = collection;
    }

    public string Title { get; set; } = "N/A";
    public List<BannerTimeDependentItem> Collection = new();

}

public class BannerTimeDependentItem
{
    public BannerTimeDependentItem() { }

    public BannerTimeDependentItem(
        int hour,
        string image)
    {
        Hour = hour;
        Image = image;
    }

    public int Hour { get; set; } = 0;
    public string Image { get; set; } = "ms-appx:///Assets/Banners/NoBanner.png";
}
