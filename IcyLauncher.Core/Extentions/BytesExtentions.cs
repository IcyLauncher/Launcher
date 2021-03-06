using Microsoft.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace IcyLauncher.Core.Extentions;

public static class BytesExtentions
{
    public static string AsString(this byte[] input) =>
        Encoding.ASCII.GetString(input);

    public static async Task<BitmapImage> AsImage(this byte[] input)
    {
        BitmapImage Result = new();
        using (var stream = new InMemoryRandomAccessStream())
        {
            await stream.WriteAsync(input.AsBuffer());
            stream.Seek(0);
            Result.SetSource(stream);
        }
        return Result;
    }
}