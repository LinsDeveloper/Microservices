

using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);

        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);

        Task<BasketCheckout> FinalCheckout(BasketCheckout checkout);

        Task DeleteBasket(string userName);
    }
}
