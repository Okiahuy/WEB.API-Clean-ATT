﻿using APPLICATIONCORE.Interface.Product;
using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _context;

        public ProductService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.supplier).Include(p => p.type).ToListAsync();
        }
        public async Task<ProductModel> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task AddProduct(ProductModel product)
        {
            if (product.ImageUpload != null)
            {
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{Path.GetFileNameWithoutExtension(product.ImageUpload.FileName)}_{timeStamp}{Path.GetExtension(product.ImageUpload.FileName)}";
                var imagePath = Path.Combine("uploads", fileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await product.ImageUpload.CopyToAsync(stream);
                }
                product.ImageUrl = $"/uploads/{fileName}"; // Gán đường dẫn vào ImageUrl
            }
            else
            {
                throw new InvalidOperationException("Ảnh về hoa là bắt buộc");
            }
            _context.Products.Add(product);
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
        public async Task<ProductModel> UpdateProductAsync(int id, ProductModel product)
        {
            // Tìm sản phẩm theo ID
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            // Lấy đường dẫn của ảnh cũ
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(existingProduct.ImageUrl));

            // Xóa ảnh cũ (nếu có)
            DeleteOldFile(oldImagePath);
            // Kiểm tra nếu có ảnh mới được tải lên
            if (product.ImageUpload != null)
            {
                // Tạo tên file với timestamp để tránh trùng lặp
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{Path.GetFileNameWithoutExtension(product.ImageUpload.FileName)}_{timeStamp}{Path.GetExtension(product.ImageUpload.FileName)}";
                var imagePath = Path.Combine("uploads", fileName);

                // Lưu file ảnh mới
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await product.ImageUpload.CopyToAsync(stream);
                }
                // Cập nhật đường dẫn ảnh mới vào sản phẩm
                existingProduct.ImageUrl = $"/uploads/{fileName}";
            }

            // Cập nhật các thuộc tính khác
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.SupplierId = product.SupplierId;
            existingProduct.TypeId = product.TypeId;
            existingProduct.DisPrice = product.DisPrice;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingProduct;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Không tìm thấy sản phẩm để xóa");

            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(product.ImageUrl));

            // Xóa ảnh cũ (nếu có)
            DeleteOldFile(oldImagePath);


            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductModel>> SearchProducts(string name)
        {
            return await _context.Products
                .Where(p => p.Name.ToString().Contains(name))
                .ToListAsync();
        }


        public async Task<IEnumerable<ProductModel>> FindById(int id)
        {
            return await _context.Products
                .Where(p => p.Id.ToString().Contains(id.ToString()))
                .ToListAsync();
        }

    }

}
