using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeGrabProductManagement.Dto;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Contexts;

namespace ShoeGrabProductManagement.Controllers;

[Route("api/admin/products")]
[Authorize(Roles = UserRole.Admin)]
public class ProductManagementController : ControllerBase
{
    private readonly ProductContext _context;
    private readonly IMapper _mapper;

    public ProductManagementController(ProductContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<ActionResult<ProductResponseDto>> AddProduct([FromBody] CreateProductDto request)
    {
        var product = _mapper.Map<Product>(request);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var responseDto = _mapper.Map<ProductResponseDto>(product);
        return Ok(responseDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponseDto>> UpdateProduct(
        int id,
        [FromBody] UpdateProductDto request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _mapper.Map(request, product);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<ProductResponseDto>(product));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
