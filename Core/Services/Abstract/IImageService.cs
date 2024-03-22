using SixLabors.ImageSharp;

namespace Core.Services.Abstract;
public interface IImageService
{
    Task<Image> ResizeImageAsync(Stream imageStream, Stream watermarkImage, Size size);
    Task<Image> ResizeImageAsync(Stream imageStream, Stream watermarkImage, int width, int height);
    Task<Image> ResizeImageAsync(Stream imageStream, Size size);
    Task<Image> ResizeImageAsync(Stream imageStream, int width, int height);

}