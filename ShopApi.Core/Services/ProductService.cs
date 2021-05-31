using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Core.Interfaces;
using ShopApi.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using ShopApi.Core.Extensions;

namespace ShopApi.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _database;
        private readonly IDistributedCache _cache;

        private static readonly string _productsKey = "products";

        public ProductService(AppDbContext context, IDistributedCache cache)
        {
            _database = context;
            _cache = cache;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _cache.GetDataAsync<List<Product>>(_productsKey);
            if (products is null)
            {
                products = await _database.Products
                                          .Include(x => x.Category)
                                          .ToListAsync();

                await _cache.AddDataAsync(_productsKey, products);
            }

            return products;
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            if (categoryId == 0)
                return await GetAllProductsAsync();

            string cacheKey = $"{_productsKey}-{categoryId}";

            var products = await _cache.GetDataAsync<List<Product>>(cacheKey);
            if (products is null)
            {
                products = await _database.Products
                                          .Where(x => x.Category.CategoryId == categoryId)
                                          .ToListAsync();

                await _cache.AddDataAsync(_productsKey, cacheKey);
            }

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _database.Products
                                  .Include(x => x.Category)
                                  .FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _database.Products.AddAsync(product);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _database.Products.Update(product);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _database.Products.Remove(product);
            await _database.SaveChangesAsync();
        }
    }
}
