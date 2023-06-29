using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Data
{
    public class ImageContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _databaseName;
        private readonly string _containerName;
        private Container _container;
        public DbSet<Models.Image> Images { get; set; }

        public ImageContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseName = _configuration["CosmosDB:DatabaseName"];
            _containerName = _configuration["CosmosDB:ContainerName"];
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var endpointUri = _configuration["CosmosDB:Uri"];
            var primaryKey = _configuration["CosmosDB:Key"];

            optionsBuilder.UseCosmos(endpointUri, primaryKey, _databaseName);
        }

        public async Task<Container> GetContainerAsync()
        {
            if (_container == null)
            {
                var cosmosClient = new CosmosClient(_configuration["CosmosDB:Uri"], _configuration["CosmosDB:Key"]);
                var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName);
                _container = await database.Database.CreateContainerIfNotExistsAsync(_containerName, "/id");
            }

            return _container;
        }
    }
}