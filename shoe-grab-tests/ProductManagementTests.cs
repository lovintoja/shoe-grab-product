using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Contexts;
using ShoeGrabProductManagement.Controllers;
using ShoeGrabProductManagement.Dto;

namespace ShoeGrabProductManagementTests;

public class ProductManagementControllerTests : IDisposable
{
    private readonly Mock<ProductContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductManagementController _controller;
    private readonly UserContextMockHelper _mockHelper;

    public ProductManagementControllerTests()
    {
        _mockHelper = new UserContextMockHelper();
        _mockContext = _mockHelper.CreateMockContext();
        _mockMapper = new Mock<IMapper>();
        _controller = new ProductManagementController(_mockContext.Object, _mockMapper.Object);

        SetupDefaultMappings();
    }



    private void SetupDefaultMappings()
    {
        _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
            .Returns((CreateProductDto dto) => new Product
            {
                Name = dto.Name,
                Price = dto.Price
            });

        _mockMapper.Setup(m => m.Map<ProductResponseDto>(It.IsAny<Product>()))
            .Returns((Product p) => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            });
    }

    public void Dispose() => _mockContext.Object.Dispose();

    [Fact]
    public async Task AddProduct_ValidRequest_ReturnsCreatedProduct()
    {
        // Arrange
        var request = new CreateProductDto
        {
            Name = "Test Product",
            Price = 99.99
        };

        // Act
        var result = await _controller.AddProduct(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ProductResponseDto>(okResult.Value);

        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Price, response.Price);
    }

    [Fact]
    public async Task UpdateProduct_NonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            Name = "New Name"
        };

        // Act
        var result = await _controller.UpdateProduct(999, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteProduct_NonExistingProduct_ReturnsNotFound()
    {
        // Act
        var result = await _controller.DeleteProduct(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
