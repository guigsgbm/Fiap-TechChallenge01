using SixLabors.ImageSharp.Formats.Jpeg;

namespace WebAPI.Services
{
    public class Services
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
        public string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }


    }
}
