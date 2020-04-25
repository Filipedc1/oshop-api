using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _database;

        public ShoppingCartService(AppDbContext context)
        {
            _database = context;
        }

        public async Task<int> CreateCartAsync(ShoppingCart cart)
        {
            await _database.ShoppingCarts.AddAsync(cart);
            await _database.SaveChangesAsync();

            return cart.ShoppingCartId;
        }

        public async Task<ShoppingCart> GetCartByIdAsync(int cartId)
        {
            return await _database.ShoppingCarts
                                  .Include(x => x.ShoppingCartItems)
                                  .FirstOrDefaultAsync(x => x.ShoppingCartId == cartId);
        }

        public async Task<ShoppingCartItem> GetItemFromCartAsync(int cartId, int productId)
        {
            var cart = await _database.ShoppingCarts
                                      .Include(x => x.ShoppingCartItems)
                                      .FirstOrDefaultAsync(x => x.ShoppingCartId == cartId);

            return cart.ShoppingCartItems.FirstOrDefault(i => i.ProductId == productId);
        }

        public async Task AddItemToCartAsync(ShoppingCartItem item)
        {
            await _database.ShoppingCartItems.AddAsync(item);
            await _database.SaveChangesAsync();
        }

        public async Task<bool> UpdateItemQuantityAsync(int productId, int quantity)
        {
            var item = await _database.ShoppingCartItems.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (item == null)
                return false;

            item.Quantity += quantity;

            await _database.SaveChangesAsync();

            return true;
        }
    }
}
