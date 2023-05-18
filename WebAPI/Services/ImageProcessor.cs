using SixLabors.ImageSharp.Formats.Jpeg;

namespace WebAPI.Services
{
    public class ImageProcessor
    {
        public byte[] ResizeImageToByteArray(byte[] imageData, int width, int heigth)
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
                    return outputStream.ToArray();
                }
            }
        }



    }
}
