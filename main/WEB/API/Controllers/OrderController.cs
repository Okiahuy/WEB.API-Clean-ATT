using APPLICATIONCORE.Interface.Email;
using APPLICATIONCORE.Interface.Order;
using APPLICATIONCORE.Models.Validation;
using APPLICATIONCORE.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public OrdersController(IOrderService orderService, IEmailService emailService)
        {
            _orderService = orderService;
            _emailService = emailService;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel orderRequest)
        {

            try
            {
                if (orderRequest == null || orderRequest.orderItemRequests == null || !orderRequest.orderItemRequests.Any())
                {
                    return BadRequest(new { message = "Danh sách sản phẩm không hợp lệ hoặc trống." });
                }

                string resultMessage = await _orderService.CreateOrderAsync(orderRequest);
                return Ok(new { message = "Đặt hàng thành công!" , data = resultMessage });
            }
            catch (Exception ex)
            {
                Log.Logger.Information(ex, "Lỗi khi tạo đơn hàng.");
                return StatusCode(500, new { message = "Có lỗi xảy ra khi Đặt hàng.", error = ex.Message });
            }
        }
        //lấy tất cả đơn hàng
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                Log.Logger.Information("{@orders}");
                return Ok(new { message = "Lấy đơn hàng thành công", data = orders });
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
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy đơn hàng.", error = ex.Message });
            }
        }

        // Lấy đơn hàng nào theo người dùng
        [HttpGet("GetOrdersByAccountID")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetOrdersByAccountID(int accountID)
        {
            var order = await _orderService.GetOrdersByAccountID(accountID);
            if (order == null || order.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng nào theo người dùng." });
            }

            Log.Logger.Information("{@order}");
            return Ok(new
            {
                message = "Tìm thấy đơn hàng nào theo người dùng.",
                data = order,
            });
        }
        //gửi mail
        [HttpPost("send-confirmation")]
        public async Task<IActionResult> SendConfirmationEmail(string to, string subject, string body)
        {
            //to = "gameclans12072003@gmail.com";
            //subject = "XIN CHÀO CỤC DÀNG";
            //body = "\n" + 
            //    "\"___♥♥♥________♥♥♥_m____\\n\" +\r\n                        " +
            //    "\"__♥______♥_♥______♥_a__\\n\" +\r\n                        " +
            //    "\"__♥_______♥_______♥_i__\\n\" +\r\n                        " +
            //    "\"___♥_____CẢM_____♥_m___\\n\" +\r\n                        " +
            //    "\"_____♥____ƠN____♥_a____\\n\" +\r\n                        " +
            //    "\"_______♥______♥_i______\\n\" +\r\n                        " +
            //    "\"_________♥___♥_________\\n\" +\r\n                        " +
            //    "\"___________♥___________\\n\";";
            await _emailService.SendEmailAsync(to, subject, body);
            Log.Logger.Information("{@email}");
            return Ok("Email đã được gửi thành công.");
        }


    }
}
