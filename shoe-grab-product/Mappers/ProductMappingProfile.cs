using AutoMapper;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Dto;

namespace ShoeGrabMonolith.Database.Mappers;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Product, ProductResponseDto>();
    }
}
