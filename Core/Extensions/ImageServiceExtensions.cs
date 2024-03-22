using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;

namespace Core.Extensions;

public static class ImageServiceExtensions
{
    public static Stream ToStream(this Image image, IImageFormat imageFormat)
    {
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, imageFormat);
        return memoryStream;
    }

    public static string ToBase64StringJPEG(this Image image)
    {
        return image.ToBase64String(JpegFormat.Instance);
    }

    public static Stream ToStreamJPEG(this Image image)
    {
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, JpegFormat.Instance);
        return memoryStream;
    }
}
