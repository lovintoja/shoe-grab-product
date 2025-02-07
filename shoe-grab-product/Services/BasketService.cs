using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Contexts;
using ShoeGrabProductManagement.Dto;

namespace ShoeGrabProductManagement.Services;

public class BasketService : IBasketService
{
    private readonly ProductContext _productContext;

    public BasketService(ProductContext productContext, IMapper mapper)
    {
        _productContext = productContext;
    }

    public async Task<Basket?> CreateBasket(int userId)
    {
        var basket = new Basket { UserId = userId };
        await _productContext.Baskets.AddAsync(basket);

        try
        {
            await _productContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return null;
        }
        return basket;
    }

    public async Task<Basket?> GetBasket(int userId)
    {
        return await _productContext.Baskets.FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task<bool> RemoveBasket(int userId)
    {
        try
        {
            var basket = await _productContext.Baskets.FindAsync(userId);

            if (basket == null)
            {
                return false;
            }

            _productContext.Baskets.Remove(basket);
            await _productContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        { 
            return false; 
        }
    }

    public async Task<bool> UpdateBasket(int userId, Basket updatedBasket)
    {
        try
        {
            if (updatedBasket == null)
            {
                throw new ArgumentNullException(nameof(updatedBasket), "Updated basket cannot be null.");
            }

            var existingBasket = await _productContext.Baskets.FirstOrDefaultAsync(b => b.UserId == userId);

            if (existingBasket == null)
            {
                existingBasket = await CreateBasket(userId);
                if (existingBasket == null)
                {
                    return false;
                }
            }
            _productContext.Entry(existingBasket).Collection(b => b.Items).IsModified = true;
            existingBasket.Items = updatedBasket.Items;

            await _productContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating basket for user {userId}: {ex.Message}");
            return false;
        }
    }
}
