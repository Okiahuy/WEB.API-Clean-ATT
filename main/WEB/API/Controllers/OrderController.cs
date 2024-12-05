using APPLICATIONCORE.Domain.Momo;
using APPLICATIONCORE.Interface.Email;
using APPLICATIONCORE.Interface.Order;
using APPLICATIONCORE.Models.Validation;
using APPLICATIONCORE.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MoMoSettings _momoSettings;

        public OrdersController(IOrderService orderService, IEmailService emailService, IHttpClientFactory httpClientFactory, MoMoSettings momoSettings)
        {
            _orderService = orderService;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
            _momoSettings = momoSettings;
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

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] OrderViewModel orderViewModel)
        {
            if (orderViewModel == null || orderViewModel.orderItemRequests == null || orderViewModel.orderItemRequests.Count == 0)
            {
                return BadRequest("Dữ liệu đơn hàng không hợp lệ.");
            }
            // Tạo thông tin thanh toán cho MoMo
            var momoRequest = new
            {
                partnerCode = "MOMO",
                accessKey = _momoSettings.AccessKey,
                orderInfo = "Thanh toán đơn hàng",
                amount = orderViewModel.totalPrice,
                orderId = GenerateRandomCode(10), // Random mã đơn hàng
                redirectUrl = _momoSettings.RedirectUrl,
                ipnUrl = _momoSettings.IpnUrl,
                requestType = "captureWallet",
                requestId = Guid.NewGuid().ToString(),
                extraData = ""
            };
            // Lưu OrderViewModel vào Session
            var orderJson = JsonConvert.SerializeObject(orderViewModel); // Serialize đối tượng thành JSON
            HttpContext.Session.SetString(momoRequest.orderId, orderJson); // Lưu JSON vào session
            Console.WriteLine($"Saved Order to Session: Key = {momoRequest.orderId}, Value = {orderJson}");



            // Tạo rawData và signature
            string rawData = $"accessKey={momoRequest.accessKey}&amount={momoRequest.amount}&extraData={momoRequest.extraData}&ipnUrl={momoRequest.ipnUrl}&orderId={momoRequest.orderId}&orderInfo={momoRequest.orderInfo}&partnerCode={momoRequest.partnerCode}&redirectUrl={momoRequest.redirectUrl}&requestId={momoRequest.requestId}&requestType={momoRequest.requestType}";
            string signature = GenerateSignature(rawData, _momoSettings.SecretKey);

            // Thêm signature vào yêu cầu
            var requestToMoMo = new
            {
                partnerCode = momoRequest.partnerCode,
                accessKey = momoRequest.accessKey,
                orderInfo = momoRequest.orderInfo,
                amount = momoRequest.amount,
                orderId = momoRequest.orderId,
                redirectUrl = momoRequest.redirectUrl,
                ipnUrl = momoRequest.ipnUrl,
                requestType = momoRequest.requestType,
                requestId = momoRequest.requestId,
                extraData = momoRequest.extraData,
                signature = signature
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(requestToMoMo);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var momoResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);

                if (momoResponse.ContainsKey("payUrl") && momoResponse.ContainsKey("qrCodeUrl"))
                {
                    var payUrl = momoResponse["payUrl"];
                    var qrCodeUrl = momoResponse["qrCodeUrl"];
                    return Ok(new { payUrl, qrCodeUrl });
                }
                return BadRequest("Không nhận được URL thanh toán từ MoMo.");
            }

            return StatusCode(500, "Yêu cầu thanh toán không thành công.");
        }
        [HttpGet("payment-success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string orderId, [FromQuery] int resultCode, [FromQuery] string message)
        {
            Console.WriteLine($"Payment Result: OrderId={orderId}, ResultCode={resultCode}, Message={message}");

            if (resultCode == 0)
            {
                // Thanh toán thành công
                try
                {
                    // Truy xuất OrderViewModel từ Session
                    var orderRequestJson = HttpContext.Session.GetString(orderId);
                    if (orderRequestJson == null)
                    {
                        return BadRequest(new { message = $"Không tìm thấy dữ liệu đơn hàng id: {orderId} trong Session: {orderRequestJson}" });
                    }

                    // Deserialize lại OrderViewModel
                    var orderRequest = JsonConvert.DeserializeObject<OrderViewModel>(orderRequestJson);

                    if (orderRequest == null || orderRequest.orderItemRequests == null || !orderRequest.orderItemRequests.Any())
                    {
                        return BadRequest(new { message = "Dữ liệu đơn hàng không hợp lệ." });
                    }
                    string resultMessage = await _orderService.CreateOrderAsync(orderRequest);
                    // Xoá session sau khi hoàn tất
                    HttpContext.Session.Remove(orderId);
                    return Ok(new { message = "Đặt hàng thành công!", data = resultMessage });
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Lỗi khi tạo đơn hàng.");
                    return StatusCode(500, new { message = "Có lỗi xảy ra khi tạo đơn hàng.", error = ex.Message });
                }
            }
            else
            {
                // Thanh toán thất bại
                return Ok(new { message = $"Thanh toán đơn hàng #{orderId} thất bại: {message}", resultCode });
            }
        }
        // Thêm API callback để xử lý kết quả thanh toán
        [HttpPost("payment-notify")]
        public IActionResult PaymentNotify([FromBody] MoMoNotifyResponse notifyResponse)
        {
            if (notifyResponse == null)
            {
                return BadRequest("Invalid notification data.");
            }

            // Kiểm tra chữ ký xác thực từ MoMo (bảo mật)
            if (!IsValidSignature(notifyResponse))
            {
                return BadRequest("Invalid signature.");
            }

            // Xử lý kết quả thanh toán
            if (notifyResponse.resultCode == 0)
            {
                // Thanh toán thành công
                // Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu
                UpdateOrderStatus(notifyResponse.orderId, "Paid");
            }
            else
            {
                // Thanh toán thất bại
                UpdateOrderStatus(notifyResponse.orderId, "Failed");
            }

            return Ok(); // Trả về 200 OK để xác nhận đã nhận thông báo
        }

        private bool IsValidSignature(MoMoNotifyResponse notifyResponse)
        {
            string rawData = $"accessKey={_momoSettings.AccessKey}&amount={notifyResponse.amount}&orderId={notifyResponse.orderId}&partnerCode={_momoSettings.PartnerCode}&requestId={notifyResponse.requestId}&resultCode={notifyResponse.resultCode}&message={notifyResponse.message}&transId={notifyResponse.transId}";
            string generatedSignature = GenerateSignature(rawData, _momoSettings.SecretKey);
            return generatedSignature == notifyResponse.signature;
        }


        private void UpdateOrderStatus(string orderId, string status)
        {
            // Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu
           
        }

        private string GenerateSignature(string rawData, string secretKey)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        // Lấy đơn hàng nào theo người dùng
        [HttpGet("GetOrdersByAccountID")]
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
            await _emailService.SendEmailAsync(to, subject, body);
            Log.Logger.Information("{@email}");
            return Ok("Email đã được gửi thành công.");
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
