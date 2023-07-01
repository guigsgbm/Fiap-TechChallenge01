using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using WebAPI.Data;
using WebAPI.Data.Dtos;
using Azure.Storage.Blobs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api")]
public class ImageController : ControllerBase
{
    private readonly ImageContext _context;
    private readonly Services.Services _services;
    private readonly IMapper _mapper;
    private readonly BlobContainerClient _blobContainerClient;

    public ImageController(BlobStorageService blobStorageService,ImageContext context, IMapper mapper, Services.Services services)
    {
        _blobContainerClient = blobStorageService.GetBlobContainerClient();
        _context = context;
        _services = services;
        _mapper = mapper;
    }

    [HttpPost("images")]
    public async Task<IActionResult> UploadImage([FromBody] CreateImageDto imageDto)
    {
        var image = _mapper.Map<Models.Image>(imageDto);

        _services.ResizeImage(image.Data, 1024, 1024);

        var container = await _context.GetContainerAsync();
        ItemResponse<Models.Image> imageResponse = await container.CreateItemAsync(image);

        Console.WriteLine($"Upload Succesfully, consumed {imageResponse.RequestCharge} RUs.\n");
        return CreatedAtAction(nameof(GetImageById), new { id = image.id }, image);
    }

    [HttpGet("images")]
    public async Task<IActionResult> GetImages([FromQuery] int skip = 0, [FromQuery] int take = 100)
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
    
    [HttpGet("images/{id}")]
    public async Task<IActionResult> GetImageById(string id)
    {
        var container = _context.GetContainerAsync();

        ItemResponse<Models.Image> imageResponse = 
            await container.Result.ReadItemAsync<Models.Image>(id, new PartitionKey(id));

        using (MemoryStream stream = new MemoryStream())
        {
            Console.WriteLine($"Consumed {imageResponse.RequestCharge} RUs.\n");
            return File(imageResponse.Resource.Data, "image/jpeg");
        }
    }
    
    [HttpDelete("images/{id}")]
    public async Task<IActionResult> DeleteImageAsync(string id)
    {
        var container = await _context.GetContainerAsync();

        var imageResponse = await container.DeleteItemAsync<Models.Image>(id, new PartitionKey(id));

        Console.WriteLine($"Delete Succesfully, consumed {imageResponse.RequestCharge} RUs.\n");
        return NoContent();
    }

    //blob

    [HttpPost("documents")]
    public async Task UploadFile(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            var fileName = file.FileName;
            await _blobContainerClient.UploadBlobAsync(fileName, stream);
        }
    }

    [HttpGet("documents/{fileName}")]
    public async Task<IActionResult> GetImages(string fileName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);

        var response = await blobClient.DownloadContentAsync();

        using (var memoryStream = new MemoryStream())
        {
            await response.Value.Content.ToStream().CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var copyStream = new MemoryStream(memoryStream.ToArray());

            return File(copyStream, response.Value.Details.ContentType, fileName); ;
        }
    }

}
