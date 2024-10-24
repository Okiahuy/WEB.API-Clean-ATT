﻿using APPLICATIONCORE.Interface.Product;
using APPLICATIONCORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // Lấy tất cả sản phẩm
        [HttpGet]
        [Authorize] // Bảo vệ route này bằng JWT
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(new { message = "Lấy sản phẩm thành công", data = products });
        }
        // Thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductModel product)
        {
            try
            {
                // Code thêm sản phẩm ở đây
                await _productService.AddProduct(product);
                return Ok(new { message = "Thêm sản phẩm thành công", data = product });
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return StatusCode(500, new { message = "Thêm sản phẩm thất bại", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Thêm sản phẩm thất bại", error = ex.Message });
            }
        }
        // Sửa sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductModel product)
        {
            try
            {
                // Gọi service để cập nhật sản phẩm
                var updatedProduct = await _productService.UpdateProductAsync(id, product);
                return Ok(new { message = "Sửa sản phẩm thành công", data = updatedProduct });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(500, new { message = "Sửa sản phẩm thất bại", ex.Message }); // Trả về lỗi nếu không tìm thấy sản phẩm
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sửa sản phẩm thất bại", ex.Message }); // Trả về lỗi nếu có vấn đề khác
            }
        }

        // Xóa sản phẩm
        [HttpDelete("search/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok(new { message = "Xóa sản phẩm thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Tìm kiếm sản phẩm theo tên
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchProducts(string name)
        {
            var products = await _productService.SearchProducts(name);
            if (products == null || products.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào" });
            }
            return Ok(new { message = "Tìm thấy sản phẩm =>", data = products });
        }

        // Tìm kiếm sản phẩm theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> findProductById(int id)
        {
            var products = await _productService.FindById(id);
            if (products == null || products.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào" });
            }
            return Ok(new { message = "Tìm thấy sản phẩm =>", data = products });
        }
    }
}

