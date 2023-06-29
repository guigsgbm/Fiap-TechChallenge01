using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class Image
{
    public Image()
    {
        id = Guid.NewGuid().ToString();
    }
    public string id { get; set; }

    [Required(ErrorMessage = $"Title is required")]
    public string Name { get; set; }

    [Required]
    public byte[] Data { get; set; }

    [Required(ErrorMessage = $"Category is required")]
    public string Category { get; set; }
}
