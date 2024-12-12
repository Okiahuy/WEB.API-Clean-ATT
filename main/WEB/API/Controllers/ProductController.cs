using APPLICATIONCORE.Interface.Product;
using APPLICATIONCORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APPLICATIONCORE.Models.Validation;
using System;
using Serilog;
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
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
				Log.Logger.Information("{@products}");
				return Ok(new { message = "Lấy sản phẩm thành công", data = products });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Bạn không có quyền truy cập." });
            }
            catch (ForbiddenAccessException)
            {
                return StatusCode(403, new { message = "Bạn không có quyền thực hiện hành động này." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy sản phẩm.", error = ex.Message });
            }
        }


        // Lấy sản phẩm theo type
        [HttpGet("GetProductsByType")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetProductsByType(int typeProduct, int page = 1, int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new { message = "Page và PageSize phải lớn hơn 0." });
            }

            var products = await _productService.GetProductsByTypeAsync(typeProduct, page, pageSize);
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào theo type." });
            }

            Log.Logger.Information("{@products}", products);
            return Ok(new
            {
                message = "Tìm thấy sản phẩm theo type.",
                data = products,
                currentPage = page,
                pageSize
            });
        }
        // Lấy sản phẩm theo danh mục
        [HttpGet("getProductByCategoryID/{categoryId}")]
        public async Task<IActionResult> getProductByCategoryID(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryID(categoryId);
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào theo danh mục" });
            }
            return Ok(new { message = "Tìm thấy sản phẩm theo danh mục =>", data = products });
        }

        // Lấy sản phẩm theo danh mục và lấy theo giá
        [HttpGet("getProductByCategoryAndPrice")]
        public async Task<IActionResult> GetProductByCategoryAndPrice(int categoryId, decimal minPrice, decimal maxPrice)
        {
            // Validate the parameters
            if (minPrice <= 0 || maxPrice <= 0 || minPrice > maxPrice)
            {
                return BadRequest(new { message = "Khoảng giá không hợp lệ." });
            }

            var products = await _productService.GetProductsByCategoryAndPriceRange(categoryId, minPrice, maxPrice);
            if (products == null || !products.Any())
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào theo danh mục và khoảng giá" });
            }

            //Log.Logger.Information("Found {@productCount} products in category {@categoryId} with price range from {@minPrice} to {@maxPrice}.", products.Count(), categoryId, minPrice, maxPrice);
            return Ok(new { message = "Tìm thấy sản phẩm theo danh mục và khoảng giá =>", data = products });
        }

        // Lấy sản phẩm theo type
        [HttpGet("getProductByCategoryIDandQuantity/{categoryId}&{sl}")]
		public async Task<IActionResult> getProductsByCategoryIDandQuantity(int categoryId, int sl)
		{
			var products = await _productService.GetProductsByCategoryIDandQuantity(categoryId, sl);
			if (products == null || products.Count == 0)
			{
				return NotFound(new { message = "Không tìm thấy sản phẩm nào theo danh mục" });
			}
			//Log.Logger.Information("{@products}");
			return Ok(new { message = "Tìm thấy sản phẩm theo danh mục =>", data = products });
		}

        //lấy sản phẩm và lọc
        [HttpGet("getProductsByCategoryAndSort")]
        public async Task<IActionResult> GetProductsByCategoryAndSort(int categoryId, decimal minPrice, decimal maxPrice, string sortBy)
        {
            var products = await _productService.GetProductsByCategoryAndPriceRange(categoryId, minPrice, maxPrice, sortBy);

            if (products == null || !products.Any())
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào theo danh mục và khoảng giá" });
            }

            return Ok(new { message = "Tìm thấy sản phẩm theo danh mục và khoảng giá", data = products });
        }

        // Lấy sản phẩm theo loai hoa hoac dung cu
        [HttpGet("GetProductByTypeForUser/{typeProduct}&{sl}")]
		public async Task<IActionResult> GetProductByTypeForUser(int typeProduct, int sl)
		{
			var products = await _productService.GetProductsByType(typeProduct, sl);
			if (products == null || products.Count == 0)
			{
				return NotFound(new { message = "Không tìm thấy sản phẩm nào theo type" });
			}
			//Log.Logger.Information("{@products}");
			return Ok(new { message = "Tìm thấy sản phẩm theo type =>", data = products });
		}
        [HttpGet("getImage/{productID}")]
        public async Task<IActionResult> GetImage(int productID)
        {
            try
            {
                // Giả sử bạn lấy thông tin sản phẩm từ cơ sở dữ liệu
                var product = await _productService.GetProductById(productID);

                if (product == null)
                {
                    return NotFound(new { message = "Sản phẩm không tồn tại" });
                }

                // Trả về hình ảnh từ URL của sản phẩm
                return Ok(new { imageURL = "http://localhost:5031" + product.ImageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy hình ảnh: {ex.Message}" });
            }
        }

        [HttpGet("getAllProductForUser")]
        public async Task<IActionResult> GetProductsForUser()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(new { message = "Lấy sản phẩm thành công", data = products });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Bạn không có quyền truy cập." });
            }
            catch (ForbiddenAccessException)
            {

                return StatusCode(403, new { message = "Bạn không có quyền thực hiện hành động này." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy sản phẩm.", error = ex.Message });
            }
        }
        // Thêm sản phẩm mới
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductModel product)
        {
            try
            {
                // Code thêm sản phẩm ở đây
                await _productService.AddProduct(product);
				Log.Logger.Information("{@products}");
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
        [Authorize(Policy = "Admin")]
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
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
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

