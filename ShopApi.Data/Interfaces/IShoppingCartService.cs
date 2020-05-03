using ShopApi.Data.Models;
using System.Threading.Tasks;

namespace ShopApi.Data.Interfaces
{
    public interface IShoppingCartService
    {
        Task<int> CreateCartAsync(ShoppingCart cart);
        Task<ShoppingCart> GetCartByIdAsync(int cartId);
        Task<ShoppingCartItem> GetItemFromCartAsync(int cartId, int productId);
        Task AddItemToCartAsync(ShoppingCartItem item);
        Task<bool> UpdateItemQuantityAsync(int productId, int quantity);
        Task ClearCartAsync(ShoppingCart cart);
        Task ClearCartAsync(int cartId);
        Task<bool> RemoveFromCartAsync(int productId);
    }
}
