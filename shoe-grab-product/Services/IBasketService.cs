using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Dto;

namespace ShoeGrabProductManagement;

public interface IBasketService
{
    Task<Basket?> GetBasket(int userId);
    Task<Basket?> CreateBasket(int userId);
    Task<bool> UpdateBasket(int userId, Basket updatedBasket);
    Task<bool> RemoveBasket(int userId);
}
