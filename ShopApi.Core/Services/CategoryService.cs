﻿using Microsoft.EntityFrameworkCore;
using ShopApi.Core.Interfaces;
using ShopApi.Data;
using ShopApi.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Core.Services
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
            return await _database.Categories
                                  .OrderBy(x => x.Name)
                                  .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _database.Categories
                                  .FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }
    }
}
