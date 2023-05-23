using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

namespace WebAPI.Validations;

public class ImageValidator
{
    private ImageContext _context;

    public ImageValidator(ImageContext context)
    {
        _context = context;
    }

    public bool SameImageName(Models.Image image)
    {
        var existingImage = _context.Images.FirstOrDefault(existingImage => existingImage.Name == image.Name);

        if (existingImage != null)
        {
            return true;
        }
        return false;
    }

    public bool ImageNotFound(Models.Image image)
    {
        if (image == null || image.Data.Length == 0 || image.Data == null)
            return true;

        return false;
    }


}
