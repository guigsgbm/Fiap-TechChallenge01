using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Dtos;
using WebAPI.Services;
using WebAPI.Validations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly ImageProcessor _imageProcessor;
    private IMapper _mapper;
    private ImageValidator _imageValidator;

    public ImageController(IMapper mapper, ImageProcessor imageProcessor, ImageValidator imageValidator)
    {
        _imageProcessor = imageProcessor;
        _mapper = mapper;
        _imageValidator = imageValidator;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage ([FromBody] CreateImageDto imageDto)
    {
        var image = _mapper.Map<Models.Image>(imageDto);
        
        using (var context = new Data.ImageContext())
        {
            if (_imageValidator.SameImageName(image))
                return Conflict($"An image with the same Name already exists");

            if (_imageValidator.ImageNotFound(image))
                return BadRequest($"Error, none image was found");

            image.Data = _imageProcessor.ResizeImageToByteArray(image.Data, 600, 600);
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

