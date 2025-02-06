using ShoeGrabCommonModels;

namespace ShoeGrabProductManagement.Dto;

public class BasketDto
{
    public List<BasketItemDto> Items { get; set; } = [];
}
