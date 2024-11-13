using APPLICATIONCORE.Interface.Order;
using APPLICATIONCORE.Models.ViewModel;
using INFRASTRUCTURE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATIONCORE.Models;
using APPLICATIONCORE.Interface.Email;

namespace INFRASTRUCTURE.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly MyDbContext _context;
        private readonly IEmailService _emailService;

        public OrderService(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<string> CreateOrderAsync(OrderViewModel orderRequest)
        {
            if (orderRequest.accountID == null || orderRequest.paymentID == null ||
                orderRequest.TotalPrice == null || orderRequest.items == null || !orderRequest.items.Any())
            {
                throw new ArgumentException("Thiếu thông tin đặt hàng.");
            }

            // Tạo mới đơn hàng
            var order = new OrderModel
            {
                code_order = GenerateRandomCode(10),
                order_date = DateTime.Now,
                accountID = orderRequest.accountID,
                PaymentID = Convert.ToInt32(orderRequest.paymentID),
                Status_order = 1, // Đơn hàng mới
                TotalPrice = Convert.ToInt32(orderRequest.TotalPrice)
            };

            // Lưu đơn hàng vào cơ sở dữ liệu
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng cho từng sản phẩm đã chọn
            foreach (var item in orderRequest.items)
            {
                var orderDetail = new OrderDetailModel
                {
                    ProductID = item.ProductID,
                    orderID = order.Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                // Lưu chi tiết đơn hàng
                await _context.OrderDetails.AddAsync(orderDetail);
            }
            await _context.SaveChangesAsync();

            // Gửi email xác nhận
            string subject = "Xác Nhận Đơn Hàng";
            string body = $"Cảm ơn bạn đã đặt hàng.\nMã đơn hàng của bạn là: {order.code_order}\nTổng tiền: {order.TotalPrice}\nNgày đặt: {order.order_date:dd-MM-yyyy}\n";
            await _emailService.SendEmailAsync(orderRequest.Email, subject, body);

            return "Đơn hàng đã được tạo thành công và gửi mail đến khách hàng!";
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
