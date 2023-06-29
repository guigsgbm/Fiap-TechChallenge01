using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace WebAPI.Services
{
    public class Services
    {
        public void ResizeImage(byte[] imageData, int width, int heigth)
        {
            using (var image = Image.Load(imageData))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, heigth),
                    Mode = ResizeMode.Max
                }));

                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, new JpegEncoder());
                    imageData = outputStream.ToArray();
                }
            }
        }
    }
}
