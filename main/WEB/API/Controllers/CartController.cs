using APPLICATIONCORE.Interface.Cart;
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
        public async Task<IActionResult> AddToCart([FromQuery] int accountID, [FromQuery] int productID, [FromQuery] int quantity)
        {
            try
            {
                await _cartService.AddToCartAsync(accountID, productID, quantity);
                return Ok(new { message = "Sản phẩm đã được thêm vào giỏ hàng", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm sản phẩm vào giỏ hàng: {ex.Message}", success = false });
            }
        }
    }
}
