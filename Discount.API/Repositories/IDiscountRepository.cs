using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> getAllDiscount();
        Task<Coupon> getDiscount(string productName);

        Task<bool> DeleteDiscount(string productName);

        Task<bool> UpdateDiscount(Coupon coupon);

        Task<bool> CreateDiscount(Coupon coupon);
    }
}
