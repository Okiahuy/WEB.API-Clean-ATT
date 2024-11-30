using APPLICATIONCORE.Interface.Cart;
using APPLICATIONCORE.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                // Truyền trực tiếp object request vào service
                await _cartService.AddToCartAsync(request.AccountID, request.ProductID, request.Quantity);
                return Ok(new { message = "Sản phẩm đã được thêm vào giỏ hàng", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm sản phẩm vào giỏ hàng: {ex.Message}", success = false });
            }
        }

        [HttpPost("UpdateToCart")]
        public async Task<IActionResult> UpdateToCart([FromBody] UpdateCartRequest request)
        {
            try
            {
                await _cartService.UpdateCartItemQuantityAsync(request.AccountID, request.ProductID, request.Quantity);
                return Ok(new { message = "Cập nhật sản phẩm thành công", success = true });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi cập nhật sản phẩm: {ex.Message}", success = false });
            }
        }

        [HttpPost("RemoveToCart")]
        public async Task<IActionResult> RemoveToCart([FromQuery] int accountID, [FromQuery] int productID, [FromQuery] int quantity)
        {
            try
            {
                await _cartService.AddToCartAsync(accountID, productID, quantity);
                return Ok(new { message = "Xóa sản phẩm thành công", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi Xóa sản phẩm thành công: {ex.Message}", success = false });
            }
        }



    }
}
