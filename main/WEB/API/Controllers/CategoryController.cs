﻿using APPLICATIONCORE.Interface.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APPLICATIONCORE.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Lấy tất cả danh mục 
        [HttpGet]
        ///[Authorize] // Bảo vệ route này bằng JWT
        public async Task<IActionResult> GetCategories()
        {
            var products = await _categoryService.GetAllCategories();
            return Ok(new { message = "Lấy danh mục thành công", data = products });
        }
        // Thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryModel product)
        {
            try
            {
                // Code thêm sản phẩm ở đây
                await _categoryService.AddCategory(product);
                return Ok(new { message = "Thêm danh mục thành công", data = product });
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return StatusCode(500, new { message = "Thêm danh mục thất bại", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Thêm danh mục thất bại", error = ex.Message });
            }
        }
        // Sửa sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryModel category)
        {
            try
            {
                var categories = await _categoryService.FindById(id);
                if (categories == null || categories.Count() == 0)
                {
                    return NotFound(new { message = "Không tìm thấy danh mục để sửa" });
                }
                // Gọi service để cập nhật sản phẩm
                var updatedcategory = await _categoryService.UpdateCategoryAsync(id, category);
                return Ok(new { message = "Sửa danh mục thành công", data = updatedcategory });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(500, new { message = "Sửa danh mục thất bại", ex.Message }); // Trả về lỗi nếu không tìm thấy sản phẩm
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sửa danh mục thất bại", ex.Message }); // Trả về lỗi nếu có vấn đề khác
            }
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var categories = await _categoryService.FindById(id);
                if (categories == null || categories.Count() == 0)
                {
                    return NotFound(new { message = "Không tìm thấy danh mục nào" });
                }

                await _categoryService.DeleteCategory(id);
                return Ok(new { message = "Xóa sản danh mục công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Tìm kiếm sản phẩm theo tên
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchCategory(string name)
        {
            var categories = await _categoryService.SearchCategories(name);
            if (categories == null || categories.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy danh mục nào" });
            }
            return Ok(new { message = "Tìm thấy sản phẩm =>", data = categories });
        }

        // Tìm kiếm sản phẩm theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> findCategoryById(int id)
        {
            var categories = await _categoryService.FindById(id);
            if (categories == null || categories.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy danh mục nào" });
            }
            return Ok(new { message = "Tìm thấy danh mục =>", data = categories });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> Search([FromQuery] string name)
        {
            var categories = await _categoryService.SearchCategories(name);
            if (categories == null || categories.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy danh mục nào" });
            }
            return Ok(new { message = "Tìm thấy danh mục =>", data = categories });
        }
    }
}
