using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using System.Collections.Generic;
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

        public async Task<Product> GetProductByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddProductAsync(Product product)
        {
            await _database.Products.AddAsync(product);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
