using AutoMapper;
using Grpc.Core;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Contexts;
using ShoeGrabProductManagement.Dto;

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

    public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.Product.Id);
        if (product == null)
            return new UpdateProductResponse { Success = false };

        try
        {
            var mappedRequest = _mapper.Map(request.Product, product);
            await _context.SaveChangesAsync();
            return new UpdateProductResponse { Success = true };
        }
        catch (Exception)
        {
            return new UpdateProductResponse { Success = false };
        }
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null)
            return new DeleteProductResponse { Success = false };

        try
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new DeleteProductResponse { Success = true };
        }
        catch (Exception)
        {
            return new DeleteProductResponse { Success = false };

        }
    }

    public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
    {
        try
        {
            var product = _mapper.Map<Product>(request.Product);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new AddProductResponse { Success = true };
        }
        catch (Exception)
        {
            return new AddProductResponse { Success = false };
        }
    }
}
