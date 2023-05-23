using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Data.Dtos;
using WebAPI.Services;
using WebAPI.Validations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private ImageContext _context;
    private readonly ImageProcessor _imageProcessor;
    private IMapper _mapper;
    private ImageValidator _imageValidator;

    public ImageController(ImageContext context,IMapper mapper, ImageProcessor imageProcessor, ImageValidator imageValidator)
    {
        _context = context;
        _imageProcessor = imageProcessor;
        _mapper = mapper;
        _imageValidator = imageValidator;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage ([FromBody] CreateImageDto imageDto)
    {
        var image = _mapper.Map<Models.Image>(imageDto);
        
        if (_imageValidator.SameImageName(image))
            return Conflict($"An image with the same Name already exists");

        if (_imageValidator.ImageNotFound(image))
            return BadRequest($"Error, none image was found");

        image.Data = _imageProcessor.ResizeImageToByteArray(image.Data, 600, 600);
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
        Console.WriteLine("Upload Succesfully");

        return CreatedAtAction(nameof(GetImageByID), new { id = image.Id }, image);
    }


    [HttpGet]
    public IActionResult GetImages([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var images = _context.Images.ToList().Skip(skip).Take(take);
        return Ok(images);
    }

    [HttpGet("{id}")]
    public IActionResult GetImageByID(int id)
    {
        var image = _context.Images.FirstOrDefault(image => image.Id == id);

        if (image != null)
            return File(image.Data, "image/jpeg");

        return NotFound();
    }

    [HttpPut("{id}")]
    public IActionResult PutImage(int id, [FromBody] UpdateImageDto imageDto)
    {
        var image = _context.Images.FirstOrDefault(image => image.Id == id);

        if (image == null)
            return NotFound();

        _mapper.Map(imageDto, image);
        _context.SaveChanges();
    
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult PatchImage(int id, JsonPatchDocument<UpdateImageDto> patch)
    {
        var image = _context.Images.FirstOrDefault(image => image.Id == id);

        if (image == null)
            return NotFound();

        var imageToUpdate = _mapper.Map<UpdateImageDto>(image);

        patch.ApplyTo(imageToUpdate, ModelState);

        if (!TryValidateModel(imageToUpdate))
            return ValidationProblem(ModelState);


        _mapper.Map(imageToUpdate, image);
        _context.SaveChanges();
        return NoContent();
    }





}

