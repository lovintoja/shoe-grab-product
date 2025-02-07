using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeGrabProductManagement.Contexts;
using ShoeGrabProductManagement.Dto;
using ShoeGrabCommonModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ShoeGrabProductManagement.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public ProductController(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }
    }
}
