using ShopApi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Data.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
