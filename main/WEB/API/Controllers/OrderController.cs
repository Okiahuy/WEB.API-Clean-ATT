using APPLICATIONCORE.Interface.Email;
using APPLICATIONCORE.Interface.Order;
using APPLICATIONCORE.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddCart([FromBody] OrderViewModel orderRequest)
        {
            try
            {
                string resultMessage = await _orderService.CreateOrderAsync(orderRequest);
                return Ok(resultMessage);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi đặt hàng.");
            }
        }

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
            return Ok("Email đã được gửi thành công.");
        }


    }
}
