using APPLICATIONCORE.Interface.Account;
using APPLICATIONCORE.Interface.Order;
using APPLICATIONCORE.Interface.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public AdminController(IProductService productService, IAccountService accountService, IOrderService orderService)
        {
            _productService = productService;
            _accountService = accountService;
            _orderService = orderService;
        }

        [HttpGet("statistics")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                // Tổng lượt thích sản phẩm
                var totalLikes = await _productService.GetTotalLikesAsync();

                // Tổng người dùng
                var totalUsers = await _accountService.GetTotalUsersAsync();

                // Tổng sản phẩm
                var totalProducts = await _productService.GetTotalProductsAsync();

                // Tổng doanh thu
                long totalPrice = await _orderService.GetTotalPrice();

                // Trả về kết quả
                return Ok(new
                {
                    TotalLikes = totalLikes,
                    TotalUsers = totalUsers,
                    TotalProducts = totalProducts,
                    TotalPrice = totalPrice
                });
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error fetching admin statistics: {@Exception}", ex);
                return StatusCode(500, $"Đã xảy ra lỗi khi lấy dữ liệu thống kê. {ex}");
            }
        }
    }

}
