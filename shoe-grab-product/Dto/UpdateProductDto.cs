using System.ComponentModel.DataAnnotations;

namespace ShoeGrabProductManagement.Dto;

public class UpdateProductDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }
}
