using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Services;

namespace WebAPI.Models;

public class Image
{
    [Key]
    [Required]
    public string id { get; set; }

    [Required(ErrorMessage = $"Title is required")]
    [StringLength(24, MinimumLength = 3, ErrorMessage = $"Title must contain between 3-24 letters.")]
    public string Name { get; set; }

    [Required]
    public byte[] Data { get; set; }

    public Image()
    {
        id = Guid.NewGuid().ToString();
    }
}