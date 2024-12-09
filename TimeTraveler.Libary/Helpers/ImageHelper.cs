using System.Drawing.Imaging;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace TimeTraveler.Libary.Helpers;

public static class ImageHelper
{
    public static Bitmap ToAvaloniaBitmap(System.Drawing.Image bitmap)
    {
        //TODO: This needs to be better..
        using (MemoryStream str = new MemoryStream())
        {
            bitmap.Save(str, ImageFormat.Png);
            str.Position = 0;
            return new Bitmap(str);
        }
    }

    public static Bitmap ToAvaloniaBitmap(MemoryStream str)
    {
        return new Bitmap(str);
    }

    public static Bitmap LoadFromResource(Uri resourceUri)
    {

        try
        {
            return new Bitmap(AssetLoader.Open(resourceUri));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
      
    }

    public static async Task<Bitmap?> LoadFromWeb(Uri url)
    {
        using var httpClient = new HttpClient();
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsByteArrayAsync();
            return new Bitmap(new MemoryStream(data));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while downloading image '{url}' : {ex.Message}");
            return null;
        }
    }
}
