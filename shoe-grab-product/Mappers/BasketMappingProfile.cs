using AutoMapper;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Dto;

namespace ShoeGrabProductManagement.Mappers;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<Basket, BasketDto>();
        CreateMap<BasketItem, BasketItemDto>();
        CreateMap<BasketDto, Basket>();
        CreateMap<BasketItemDto, BasketItem>();
    }
}
