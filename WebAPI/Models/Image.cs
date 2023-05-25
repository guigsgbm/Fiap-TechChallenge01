using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Services;

namespace WebAPI.Models;

public class Image
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = $"Title is required")]
    [StringLength(24, MinimumLength = 3, ErrorMessage = $"Title must contain between 3-24 letters.")]
    public string Name { get; set; }

    [Required]
    public byte[] Data { get; set; }

    [Required]
    public string Path { get; set; }
}