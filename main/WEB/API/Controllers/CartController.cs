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

        [HttpGet("getCartbyID/{accountID}")]
        public async Task<IActionResult> GetCartByID(int accountID)
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var cart = await _cartService.GetCartByAccountIDAsync(accountID);
                // Tính tổng số lượng sản phẩm trong giỏ hàng
                var totalItems = cart.Sum(c => c.cartID);
                var totalPrice = cart.Sum(c => c.TotalPrice);


                if (cart == null || !cart.Any())
                {
                    return NotFound(new {
                        message = "Không tìm thấy sản phẩm nào",
                        totalItems=0, // Tổng số lượng sản phẩm trong giỏ hàng
                        totalPrice=0
                    });
                }

                return Ok(new
                {
                    message = "Tìm thấy sản phẩm =>",
                    data = cart,
                    totalItems, // Tổng số lượng sản phẩm trong giỏ hàng
                    totalPrice
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy sản phẩm trong giỏ hàng: {ex.Message}", success = false });
            }
        }


        [HttpPost("UpdateToCart")]
        public async Task<IActionResult> UpdateToCart([FromBody] UpdateCartRequest request)
        {
            try
            {
                // Cập nhật số lượng sản phẩm trong giỏ hàng
                await _cartService.UpdateCartItemQuantityAsync(request.AccountID, request.ProductID, request.Quantity);

                // Lấy giỏ hàng mới sau khi cập nhật
                var cart = await _cartService.GetCartByAccountIDAsync(request.AccountID);

                if (cart == null || !cart.Any())
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm nào trong giỏ hàng", success = false });
                }

                // Tính tổng số lượng sản phẩm và tổng giá trị
                var totalItems = cart.Sum(c => c.cartID);
                var totalPrice = cart.Sum(c => c.TotalPrice);

                return Ok(new
                {
                    message = "Cập nhật sản phẩm thành công",
                    data = cart,
                    totalItems, // Tổng số lượng sản phẩm
                    totalPrice // Tổng giá trị giỏ hàng
                });
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


        [HttpDelete("RemoveToCart")]
        public async Task<IActionResult> RemoveToCart([FromQuery] int cartID, [FromQuery] int accountID)
        {
            try
            {
                // Call the service to remove the product from the cart
                await _cartService.RemoveFromCartAsync(cartID);

                var cart = await _cartService.GetCartByAccountIDAsync(accountID);

                if (cart == null)
                {
                    return Ok(new {
                        message = "Giỏ hàng hiện không có sản phẩm nào",
                        data = cart,
                        totalItems = 0,
                        totalPrice = 0 
                    });
                }

                // Tính tổng số lượng sản phẩm và tổng giá trị
                var totalItems = cart.Sum(c => c.cartID);
                var totalPrice = cart.Sum(c => c.TotalPrice);

                return Ok(new
                {
                    message = "Xóa sản phẩm thành công",
                    success = true,
                    data = cart,
                    totalItems, // Tổng số lượng sản phẩm
                    totalPrice // Tổng giá trị giỏ hàng
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa sản phẩm: {ex.Message}", success = false });
            }
        }






    }
}
