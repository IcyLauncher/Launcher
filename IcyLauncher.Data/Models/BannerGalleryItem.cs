using System.Collections.Generic;

namespace IcyLauncher.Data.Models;

public class BannerGalleryItem
{
    public BannerGalleryItem() { }

    public BannerGalleryItem(string title, List<string> collection)
    {
        Title = title;
        Collection = collection;
    }

    public string Title { get; set; } = "N/A";
    public List<string> Collection { get; set; } = new();
}