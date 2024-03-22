using Core.Services.Abstract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Core.Services;
public class ImageService : IImageService
{
    public async Task<Image> ResizeImageAsync(Stream imageStream, Stream watermarkImage, Size size)
    {
        var image = await Image.LoadAsync(imageStream);

        image.Mutate(p =>
        {
            p.Resize(new ResizeOptions
            {
                Size = size,
                Mode = ResizeMode.Pad
            });
        });
        image.Mutate(p =>
        {
            var watermark = Image.Load(watermarkImage);
            p.DrawImage(watermark, 0.3f);
        });
        return image;
    }

    public async Task<Image> ResizeImageAsync(Stream imageStream, Stream watermarkImage, int width, int height)
    {
        return await ResizeImageAsync(imageStream, watermarkImage, new Size(width, height));
    }

    public async Task<Image> ResizeImageAsync(Stream imageStream, Size size)
    {
        var image = await Image.LoadAsync(imageStream);

        image.Mutate(p =>
        {
            p.Resize(new ResizeOptions
            {
                Size = size,
                Mode = ResizeMode.Pad
            });
        });
        return image;
    }

    public async Task<Image> ResizeImageAsync(Stream imageStream, int width, int height)
    {
        return await ResizeImageAsync(imageStream, new Size(width, height));
    }
}
