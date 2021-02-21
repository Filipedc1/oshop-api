using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _database;

        public ProductService(AppDbContext context)
        {
            _database = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _database.Products
                                  .Include(x => x.Category)
                                  .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            if (categoryId == 0)
                return await GetAllProductsAsync();

            return await _database.Products
                                  .Where(x => x.Category.CategoryId == categoryId)
                                  .ToListAsync();
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
