using APPLICATIONCORE.Interface.Product;
using APPLICATIONCORE.Models;
using APPLICATIONCORE.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace INFRASTRUCTURE.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<long> GetTotalLikesAsync()
        {
            return await _context.Products.SumAsync(p => p.likecount ?? 0);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }

        //lấy tất cả sp
        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.supplier).Include(p => p.type).ToListAsync();
        }
        //lấy sp theo id
        public async Task<ProductModel> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        //lấy sp theo giá
        public async Task<IEnumerable<ProductModel>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .Distinct()
                .ToListAsync();
        }
        //lấy sp theo giá và theo danh mục
        public async Task<IEnumerable<ProductModel>> GetProductsByCategoryAndPriceRange(int categoryId, decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.Price >= minPrice && p.Price <= maxPrice)
                .Distinct()
                .ToListAsync();

        }
        //lấy sp theo danh mục id
        public async Task<List<ProductModel>> GetProductsByCategoryID(int categoryId)
		{
			return await _context.Products
								 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
		}
        //lấy sp theo danh mục id và số lượng
        public async Task<List<ProductModel>> GetProductsByCategoryIDandQuantity(int categoryId, int sl)
        {
            return await _context.Products
                          .Where(p => p.CategoryId == categoryId)
                          .Take(sl)
                          .Distinct()
                          .OrderBy(p => p.Id)
                          .ToListAsync();
        }
        //lấy sp theo danh mục và sắp xếp
        public async Task<List<ProductModel>> GetProductsByCategoryAndPriceRange(int categoryId, decimal minPrice, decimal maxPrice, string sortBy)
        {
            var query = _context.Products
                .Where(p => p.CategoryId == categoryId && p.Price >= minPrice && p.Price <= maxPrice); // Lọc theo danh mục và giá

            // Áp dụng sắp xếp dựa trên tham số sortBy
            switch (sortBy)
            {
                case "name-asc":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "name-desc":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                case "price-asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    break;
            }

            // Trả về danh sách sản phẩm đã được lọc và sắp xếp
            return await query.ToListAsync();
        }


        //lấy sp theo danh mục type
        public async Task<List<ProductModel>> GetProductsByType(int typeProduct, int sl)
		{
			return await _context.Products
								 .Where(p => p.typeProduct == typeProduct)
                                 .Take(sl)
                                 .ToListAsync();
		}
        //lấy sản phẩm theo typeProduct và phân trang
        public async Task<List<ProductModel>> GetProductsByTypeAsync(int typeProduct, int page, int pageSize = 5)
        {
            return await _context.Products
                                 .Where(p => p.typeProduct == typeProduct)
                                 .Skip((page - 1) * pageSize) // Bỏ qua số lượng sản phẩm trước đó
                                 .Take(pageSize)
                                 .ToListAsync();
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


			// Lấy roleID từ token của người dùng
			var roleIDClaim = _httpContextAccessor.HttpContext?.User.FindFirst("roleID")?.Value;
           
            product.CreatedByRoleID = Convert.ToInt32(roleIDClaim);
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
                throw new KeyNotFoundException("Không tìm thấy sản phẩm để sửa!");
            }
            // Lấy đường dẫn của ảnh cũ
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(existingProduct.ImageUrl));
            // Lấy roleID của admin hiện tại từ token
            var roleIDClaim = _httpContextAccessor.HttpContext?.User.FindFirst("roleID")?.Value;
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
            existingProduct.Quantity = product.Quantity;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.SupplierId = product.SupplierId;
            existingProduct.TypeId = product.TypeId;
            existingProduct.DisPrice = product.DisPrice;
			existingProduct.typeProduct = product.typeProduct;
			existingProduct.UpdatedByRoleID = Convert.ToInt32(roleIDClaim);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingProduct;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            // Lấy roleID của admin hiện tại từ token
            var roleIDClaim = _httpContextAccessor.HttpContext?.User.FindFirst("roleID")?.Value;

            if (product == null) throw new KeyNotFoundException("Không tìm thấy sản phẩm để xóa");

            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(product.ImageUrl));

            // Xóa ảnh cũ (nếu có)
            DeleteOldFile(oldImagePath);

            // Lưu thông tin vào ProductHistory trước khi xóa
            var productHistory = new ProductHistory
            {
                ProductID = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Quantity = Convert.ToInt32(product.Quantity),
                DeletedAt = DateTime.UtcNow,
                DeletedByRoleID = Convert.ToInt32(roleIDClaim)
            };

            _context.ProductHistory.Add(productHistory); // Thêm vào bảng ProductHistory
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
                .Include(p => p.Category)    // Load thông tin của Category
                .Include(p => p.supplier)    // Load thông tin của Supplier
                .Include(p => p.type)        // Load thông tin của Type
                .Where(p => p.Id.ToString().Contains(id.ToString()))
                .ToListAsync();
        }


    }

}

