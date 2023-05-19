using System.ComponentModel.DataAnnotations;

namespace WebAPI.Data.Dtos;

public class CreateImageDto
{
    [Required(ErrorMessage = $"Title is required")]
    [StringLength(24, MinimumLength = 3, ErrorMessage = $"Title must contain between 3-24 letters.")]
    public string Name { get; set; }

    [Required]
    public byte[] Data { get; set; }

}
