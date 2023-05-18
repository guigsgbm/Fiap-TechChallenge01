using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class ImageContext : DbContext
    {
        public DbSet<Models.Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opts)
        {
            opts.UseInMemoryDatabase("ImageDB");
        }

    }
}
