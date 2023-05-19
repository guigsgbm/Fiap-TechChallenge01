using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

namespace WebAPI.Validations;

public class ImageValidator
{
    private ControllerBase _controller;
    private ImageContext _context;

    public ImageValidator(ControllerBase controller, ImageContext context)
    {
        _controller = controller;
        _context = context;
    }

    public void SameImageName(Models.Image image)
    {
        var existingImage = _context.Images.FirstOrDefault(existingImage => existingImage.Name == image.Name);
        if (existingImage != null)
        {
            _controller.Conflict("An image with the same Name already exists");
        }
    }

    public void ImageNotFound(Models.Image image)
    {
        if (image == null || image.Data.Length == 0 || image.Data == null)
            _controller.BadRequest("Error, none image was found");
    }


}
