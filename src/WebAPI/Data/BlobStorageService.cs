using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Data;

public class BlobStorageService
{
    private readonly BlobContainerClient _blobContainerClient;
    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("BlobStorage:ConnectionString").Value;
        var containerName = configuration.GetSection("BlobStorage:ContainerName").Value;
        _blobContainerClient = new BlobContainerClient(connectionString, containerName);
    }

    public BlobContainerClient GetBlobContainerClient()
    {
        return _blobContainerClient;
    }
}
