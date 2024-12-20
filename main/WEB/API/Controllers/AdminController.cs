using APPLICATIONCORE.Interface.Account;
using APPLICATIONCORE.Interface.Order;
using APPLICATIONCORE.Interface.Product;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly MyDbContext _context;

        public AdminController(IProductService productService, IAccountService accountService, IOrderService orderService, MyDbContext context)
        {
            _productService = productService;
            _accountService = accountService;
            _orderService = orderService;
            _context = context;
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
        //lấy hóa đơn
        [HttpGet("GetInvoice")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetInvoice()
        {
            try
            {
                // hiên thị thông tin của hóa đơn
                var mid = await _context.Invoices
                                 .Include(o => o.Order)
                                 .ToListAsync();
                // Trả về kết quả
                return Ok(new
                {
                    data = mid,
                });
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error fetching admin GetInvoice: {@Exception}", ex);
                return StatusCode(500, $"Đã xảy ra lỗi khi lấy dữ liệu thống kê. {ex}");
            }
        }

        [HttpGet("getMiddleware")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetMiddleWare()
        {
            try
            {
                // hiên thị thông tin của web
                var mid = await _context.ApiUsageLogs.ToListAsync();
                //tổng truy cập
                var Access = await _context.ApiUsageLogs.SumAsync(p => p.RequestCount ?? 0);
                // cpu trung bình
                var cpu = await _context.ApiUsageLogs.AverageAsync(p => p.CpuUsage);
                //thười gian request trung bình
                var requestTime = await _context.ApiUsageLogs
                    .Select(log => log.RequestTime.TotalMilliseconds) // Lấy giá trị cần thiết
                    .ToListAsync(); // Chuyển dữ liệu sang client-side

                var averageRequestTime = requestTime.Average(); // Tính trung bình trên client-side

                // Trả về kết quả
                return Ok(new
                {
                    data = mid,
                    TotalAccess = Access,
                    cpuavg = cpu,
                    requestTime = averageRequestTime
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
