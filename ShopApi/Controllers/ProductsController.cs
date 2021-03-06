﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Core.Dto;
using Microsoft.Extensions.Logging;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper,
                                  ILogger<ProductsController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET  /products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                if (products == null)
                    return NotFound();

                var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET  /products/category/1
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);

            if (products == null)
                return NotFound();

            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(dtos);
        }

        // GET  /products/12
        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
                return NotFound();

            var dto = _mapper.Map<ProductDto>(product);

            return Ok(dto);
        }

        // POST  /products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddProductAsync(ProductDto productDto)
        {
            if (productDto is null) return NotFound();

            try
            {
                var product = new Product
                {
                    ProductId = productDto.ProductId,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    ImageUrl = productDto.ImageUrl,
                    Category = await _categoryService.GetCategoryByIdAsync(productDto.CategoryId)
                };

                await _productService.AddProductAsync(product);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }

            return Ok();
        }

        // PUT  /products/12
        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProductAsync(int productId, ProductDto productDto)
        {
            if (productDto == null) 
                return BadRequest();

            try
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null) 
                    return NotFound();

                product.Name = productDto.Name;
                product.Price = productDto.Price;
                product.ImageUrl = productDto.ImageUrl;
                product.Category = await _categoryService.GetCategoryByIdAsync(productDto.CategoryId);

                await _productService.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE  /products/12
        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProductAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound();

            try
            {
                await _productService.DeleteProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }

            return Ok();
        }
    }
}