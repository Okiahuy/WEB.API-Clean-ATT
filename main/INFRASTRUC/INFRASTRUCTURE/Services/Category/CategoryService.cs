using APPLICATIONCORE.Interface.Category;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Category
{
    public class CategoryService : ICategoryService
    {

        private readonly MyDbContext _context;
        public CategoryService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryModel>> GetAllCategories()
        {
            return await _context.Categories.Distinct().ToListAsync();
        }
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task AddCategory(CategoryModel category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        private void BadRequest(object value)
        {
            throw new NotImplementedException();
        }

        //hàm cập nhật sản phẩm
        public async Task<CategoryModel> UpdateCategoryAsync(int id, CategoryModel category)
        {
            // Tìm sản phẩm theo ID
            var existingcategory = await _context.Categories.FindAsync(id);
            if (existingcategory == null)
            {
                throw new KeyNotFoundException("Không tìm thấy danh mục");
            }
            // Cập nhật các thuộc tính khác
            existingcategory.Name = category.Name;
            existingcategory.Description = category.Description;


            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingcategory;
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new KeyNotFoundException("Không tìm thấy sản phẩm để xóa");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<CategoryModel>> SearchCategories(string name)
        {
            return await _context.Categories
                .Where(p => p.Name.ToString().Contains(name))
                .ToListAsync();
        }
        public async Task<IEnumerable<CategoryModel>> FindById(int id)
        {
            return await _context.Categories
                .Where(p => p.Id.ToString().Contains(id.ToString()))
                .ToListAsync();
        }
        public async Task<IEnumerable<ProductModel>> FindProductById(int id)
        {
            // Direct equality comparison for accurate results
            return await _context.Products
                .Where(p => p.CategoryId == id)
                .ToListAsync();
        }
    }
}
