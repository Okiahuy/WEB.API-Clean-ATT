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
using Microsoft.EntityFrameworkCore;
using Serilog;

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
            try
            {
                // Check if the account exists
                var accountExists = await _context.Accounts
                    .Where(a => a.accountID == orderRequest.accountID)
                    .Select(a => a.accountID)
                    .FirstOrDefaultAsync();

                if (accountExists == null)
                {
                    throw new ArgumentException("Tài khoản không hợp lệ.");
                }

                // Create new order
                var order = new OrderModel
                {
                    code_order = GenerateRandomCode(10),
                    order_date = DateTime.Now,
                    accountID = orderRequest.accountID, // Use the provided account ID
                    PaymentID = orderRequest.paymentID,
                    Status_order = 1, // New order status
                    TotalPrice = orderRequest.totalPrice
                };

                // Save order to the database
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Create order details for each product in the order
                foreach (var item in orderRequest.orderItemRequests)
                {
                    if (item.productID <= 0 || item.quantity <= 0 || item.price <= 0)
                    {
                        throw new ArgumentException("Thông tin sản phẩm không hợp lệ.");
                    }

                    var orderDetail = new OrderDetailModel
                    {
                        ProductID = item.productID,
                        orderID = order.Id,
                        Quantity = item.quantity,
                        Price = item.price,
                        Status_order = 1
                    };

                    // Save order details to the database
                    await _context.OrderDetails.AddAsync(orderDetail);

                    await ClearCartAsync(orderRequest.accountID, item.productID);
                }
                await _context.SaveChangesAsync();

                // Send confirmation email
                string subject = "Xác Nhận Đơn Hàng";
                string body = $"Cảm ơn bạn đã đặt hàng.\nMã đơn hàng của bạn là: {order.code_order}\nTổng tiền: {order.TotalPrice}\nNgày đặt: {order.order_date:dd-MM-yyyy}\n\n\n" +
                    "\n"  + "\n\n" +
                    "\"___♥♥♥________♥♥♥_m____\\n\" +\r\n                        " +
                    "\"__♥______♥_♥______♥_a__\\n\" +\r\n                        " +
                    "\"__♥_______♥_______♥_i__\\n\" +\r\n                        " +
                    "\"___♥_____CẢM_____♥_m___\\n\" +\r\n                        " +
                    "\"_____♥____ƠN____♥_a____\\n\" +\r\n                        " +
                    "\"_______♥______♥_i______\\n\" +\r\n                        " +
                    "\"_________♥___♥_________\\n\" +\r\n                        " +
                    "\"___________♥___________\\n\";";
                await _emailService.SendEmailAsync(orderRequest.email, subject, body);

                return "Đơn hàng đã được tạo thành công và gửi mail đến khách hàng!";
            }
            catch (DbUpdateException ex)
            {
                // Log detailed error message
                Log.Logger.Information(ex, "Database update exception occurred while creating order.");
                throw new Exception("Có lỗi xảy ra khi Đặt hàng. Chi tiết: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                // Log the error message
                Log.Logger.Information(ex, "General error occurred while creating order.");
                throw new Exception("Có lỗi xảy ra khi Đặt hàng.");
            }
        }
        private async Task ClearCartAsync(int accountID, int productID)
        {
            // Assuming there is a Cart table that holds cart items for users
            var cartItems = await _context.Carts.Where(c => c.accountID == accountID && c.productID == productID).ToListAsync();

            if (cartItems.Any())
            {
                _context.Carts.RemoveRange(cartItems); // Removes all cart items for the user
                await _context.SaveChangesAsync(); // Save changes to remove items from the cart
            }
        }
        //lấy tất cả don hang
        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            return await _context.Orders.Include(p => p.Account).ToListAsync();
        }
        //lấy sp theo id
        public async Task<OrderModel> GetOrderById(int Id)
        {
            return await _context.Orders.FindAsync(Id);
        }
        //lấy sp theo danh mục id
        public async Task<List<OrderModel>> GetOrdersByAccountID(int accountID)
        {
            return await _context.Orders
                                 .Where(p => p.accountID == accountID)
                                 .ToListAsync();
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
