using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly ImageProcessor _imageProcessor = new();

    [HttpPost]
    public async Task<IActionResult> UploadImage ([FromBody] Models.Image image)
    {
        if (image == null || image.Data.Length == 0 || image.Data == null)
            return BadRequest("Error, none image was found");


        using (var context = new Data.ImageContext())
        {
            var existingImage = context.Images.FirstOrDefault(existingImage => existingImage.Name == image.Name);
            if (existingImage != null)
            {
                return Conflict("An image with the same Name already exists");
            }

            image.Data = _imageProcessor.ResizeImageToByteArray(image.Data, 800, 600);
            context.Images.Add(image);
            await context.SaveChangesAsync();
            Console.WriteLine("Upload Succesfully");
        }

        return CreatedAtAction(nameof(GetImageByID), new { id = image.Id }, image);
    }


    [HttpGet]
    public async Task<IActionResult> GetImages([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        using (var context = new Data.ImageContext())
        {
            var images = context.Images.ToList().Skip(skip).Take(take);
            return Ok(images);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageByID (int id)
    {
        using (var context = new Data.ImageContext())
        {
            var image = context.Images.FirstOrDefault(image => image.Id == id);

            if (image != null)
                return File(image.Data, "image/jpeg");
            
            return NotFound();
        }
    }
}

