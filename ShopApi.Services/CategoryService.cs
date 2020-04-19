using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _database;

        public CategoryService(AppDbContext context)
        {
            _database = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _database.Categories.ToListAsync();
        }
    }
}
