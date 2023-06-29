using System.ComponentModel.DataAnnotations;

namespace WebAPI.Data.Dtos;

public class CreateImageDto
{
    [Required(ErrorMessage = $"Title is required")]
    public string Name { get; set; }

    public byte[] Data { get; set; }
    public string Category { get; set; }

}
