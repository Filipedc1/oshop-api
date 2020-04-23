﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using ShopApi.DTOs;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        // GET  /products
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();

            if (products == null)
                return NotFound();

            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(dtos);
        }

        // GET  /products/12
        [HttpGet("{productId}")]
        [AllowAnonymous]
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
            catch(Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT  /products/12
        [HttpPut("{productId}")]
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
            catch (Exception e)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE  /products/12
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProductAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound();

            try
            {
                await _productService.DeleteProductAsync(product);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}