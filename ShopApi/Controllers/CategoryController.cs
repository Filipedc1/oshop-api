using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Interfaces;
using ShopApi.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET  /category
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            if (categories == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }
    }
}