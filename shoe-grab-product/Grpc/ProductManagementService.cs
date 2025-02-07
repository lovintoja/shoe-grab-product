using AutoMapper;
using Grpc.Core;
using ShoeGrabProductManagement.Contexts;

namespace ShoeGrabProductManagement.Grpc;

public class ProductManagementService : ProductManagement.ProductManagementBase
{
    private readonly ProductContext _context;
    private readonly IMapper _mapper;

    public ProductManagementService(ProductContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.Id);
        return new GetProductResponse { Product = _mapper.Map<ProductProto>(product) };
    }
}
