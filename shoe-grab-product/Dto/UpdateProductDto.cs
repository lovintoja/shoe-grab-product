using System.ComponentModel.DataAnnotations;

namespace ShoeGrabProductManagement.Dto;

public class UpdateProductDto
{
    [Required]
    public int Id { get; set; }
    [StringLength(100)]
    public string? Name { get; set; }

    [Range(0.01, double.MaxValue)]
    public double Price { get; set; }
}
