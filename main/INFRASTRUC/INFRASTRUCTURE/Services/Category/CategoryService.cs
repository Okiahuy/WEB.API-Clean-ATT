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
            if (category.ImageUpload != null)
            {
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{Path.GetFileNameWithoutExtension(category.ImageUpload.FileName)}_{timeStamp}{Path.GetExtension(category.ImageUpload.FileName)}";
                var imagePath = Path.Combine("uploads", fileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await category.ImageUpload.CopyToAsync(stream);
                }
                category.ImageUrl = $"/uploads/{fileName}"; // Gán đường dẫn vào ImageUrl
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        private void BadRequest(object value)
        {
            throw new NotImplementedException();
        }
        private void DeleteOldFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

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
            // Lấy đường dẫn của ảnh cũ
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(existingcategory.ImageUrl));
            // Xóa ảnh cũ (nếu có)
            DeleteOldFile(oldImagePath);
            // Kiểm tra nếu có ảnh mới được tải lên
            if (category.ImageUpload != null)
            {
                // Tạo tên file với timestamp để tránh trùng lặp
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{Path.GetFileNameWithoutExtension(category.ImageUpload.FileName)}_{timeStamp}{Path.GetExtension(category.ImageUpload.FileName)}";
                var imagePath = Path.Combine("uploads", fileName);

                // Lưu file ảnh mới
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await category.ImageUpload.CopyToAsync(stream);
                }
                // Cập nhật đường dẫn ảnh mới vào sản phẩm
                existingcategory.ImageUrl = $"/uploads/{fileName}";
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
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(category.ImageUrl));

            // Xóa ảnh cũ (nếu có)
            DeleteOldFile(oldImagePath);
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
