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

        public Task<List<Product>> GetAllProductsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddProductAsync(Product product)
        {
            await _database.AddAsync(product);
            await _database.SaveChangesAsync();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
