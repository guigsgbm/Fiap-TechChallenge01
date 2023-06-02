using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using WebAPI.Data;
using WebAPI.Data.Dtos;
using WebAPI.Services;
using WebAPI.Validations;
using System.IO;
using System.Net.Mime;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly ImageContext _context;
    private readonly Services.Services _services;
    private readonly IMapper _mapper;
    private readonly ImageValidator _imageValidator;

    public ImageController(ImageContext context, IMapper mapper, Services.Services services, ImageValidator imageValidator)
    {
        _context = context;
        _services = services;
        _mapper = mapper;
        _imageValidator = imageValidator;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage([FromBody] CreateImageDto imageDto)
    {
        var image = _mapper.Map<Models.Image>(imageDto);
        image.Data = Convert.FromBase64String(_services.ImageToBase64(imageDto.Path));

       // if (_imageValidator.SameImageName(image))
          //  return Conflict($"An image with the same Name already exists");

        //if (_imageValidator.ImageNotFound(image))
         //   return BadRequest($"Error, none image was found");

        image.Data = _services.ResizeImageToByteArray(image.Data, 400, 400);

        var container = await _context.GetContainerAsync();
        ItemResponse<Models.Image> imageResponse = await container.CreateItemAsync(image);

        Console.WriteLine($"Upload Succesfully, consumed {imageResponse.RequestCharge} RUs.\n");
        return CreatedAtAction(nameof(GetImageById), new { id = image.id }, image);
    }

    [HttpGet]
    public async Task<IActionResult> GetImages([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var container = await _context.GetContainerAsync();

        // GetItemLinqQueryable retorna <dynamic> e não ItemResponse, por isso não tem RequestCharge
        var imagesResponse = container.GetItemLinqQueryable<dynamic>
            (allowSynchronousQueryExecution: true)
            .ToList()
            .Skip(skip)
            .Take(take);

        var images = _mapper.Map<IEnumerable<ReadImageDto>>(imagesResponse);

        if (!images.Any())
            return Ok($"No one item found");

        return Ok(images);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(string id)
    {
        var container = _context.GetContainerAsync();

        ItemResponse<Models.Image> imageResponse = 
            await container.Result.ReadItemAsync<Models.Image>(id, new PartitionKey(id));

        if (imageResponse == null)
            return NotFound();

        using (MemoryStream stream = new MemoryStream())
        {
            Console.WriteLine($"Consumed {imageResponse.RequestCharge} RUs.\n");
            return File(imageResponse.Resource.Data, "image/jpeg");
        }
    }
    
    

    [HttpPut("{id}")]
    public async Task<IActionResult> PutImageAsync(string id, [FromBody] UpdateImageDto imageDto)
    {
        var container = await _context.GetContainerAsync();

        ItemResponse<Models.Image> image =
                    await container.ReadItemAsync<Models.Image>(id, new PartitionKey(id));

        if (image == null)
            return NotFound();
        
        var updatedImage = _mapper.Map<Models.Image>(image);
        updatedImage.Data = Convert.FromBase64String(_services.ImageToBase64(imageDto.Path));
        updatedImage = _mapper.Map(imageDto, updatedImage);

        updatedImage.Data = _services.ResizeImageToByteArray(updatedImage.Data, 400, 400);

        var imageResponse = await container.ReplaceItemAsync(updatedImage, updatedImage.id);

        Console.WriteLine($"Put Succesfully, consumed { imageResponse.RequestCharge} RUs.\n");
        return NoContent();
    }

    /*
    [HttpPatch("{id}")]
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
    */

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImageAsync(string id)
    {
        var container = await _context.GetContainerAsync();

        var imageResponse = await container.DeleteItemAsync<Models.Image>(id, new PartitionKey(id));

        Console.WriteLine($"Delete Succesfully, consumed {imageResponse.RequestCharge} RUs.\n");
        return NoContent();
    }
}
