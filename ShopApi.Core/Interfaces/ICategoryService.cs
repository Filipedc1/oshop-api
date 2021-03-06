﻿using ShopApi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
