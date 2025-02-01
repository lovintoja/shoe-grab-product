using AutoMapper;
using ShoeGrabCommonModels;

namespace ShoeGrabProductManagement.Mappers;

public class GrpcMappingProfile : Profile
{
    public GrpcMappingProfile()
    {
        CreateMap<Product, ProductProto>();
    }
}
