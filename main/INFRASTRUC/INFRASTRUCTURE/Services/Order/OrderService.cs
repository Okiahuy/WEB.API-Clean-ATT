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
using APPLICATIONCORE.Domain.Momo.MomoDtos;
using MailKit.Search;
using Microsoft.Identity.Client;

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

        public async Task<long> GetTotalPrice()
        {
            return await _context.Orders
                .Where(p => p.Status_order == 4)
                .SumAsync(p => Convert.ToInt64(p.TotalPrice));
        }

        //tạo đơn hàng 
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
                    TotalPrice = orderRequest.totalPrice,
                    addressID = orderRequest.addressID,
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
                string subject = "Xác Nhận Đơn Hàng Của Bạn";
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

                var noti = new Notification
                {
                    accountID = orderRequest.accountID,
                    Create = DateTime.Now,
                    description = $"Cảm ơn bạn đã đặt hàng. Mã đơn hàng của bạn là: { order.code_order}\nTổng tiền: { order.TotalPrice}\nNgày đặt: { order.order_date:dd - MM - yyyy}\n\n\n"
                };
                await _context.Notis.AddAsync(noti);
                await _context.SaveChangesAsync();

                return "Đơn hàng đã được tạo thành công và gửi mail đến bạn!";
            }
            catch (DbUpdateException ex)
            {
                // Log detailed error message
                //Log.Logger.Information(ex, "Database update exception occurred while creating order.");
                throw new Exception("Có lỗi xảy ra khi Đặt hàng. Chi tiết: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                // Log the error message
                //Log.Logger.Information(ex, "General error occurred while creating order.");
                throw new Exception("Có lỗi xảy ra khi Đặt hàng.");
            }
        }
        public async Task<string> CreatePaymentMomoAsync(OrderViewModel orderRequest, string orderId)
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

                // Create new momo
                var order = new MomoDTO
                {
                    code_order = orderId,
                    order_date = DateTime.Now,
                    accountID = orderRequest.accountID, 
                    PaymentID = orderRequest.paymentID,
                    PaymentName = "MOMO", 
                    TotalPrice = orderRequest.totalPrice,
                    email = orderRequest.email,
                    addressID = orderRequest.addressID,
                };

                // Save order to the database
                await _context.Momos.AddAsync(order);
                await _context.SaveChangesAsync();

                // Create order details for each product in the order
                foreach (var item in orderRequest.orderItemRequests)
                {
                    if (item.productID <= 0 || item.quantity <= 0 || item.price <= 0)
                    {
                        throw new ArgumentException("Thông tin sản phẩm không hợp lệ.");
                    }

                    var orderDetail = new MomodetailDTO
                    {
                        ProductID = item.productID,
                        orderID = order.Id,
                        Quantity = item.quantity,
                        Price = item.price,
                        code_order = order.code_order
                    };
                    await _context.MomoDetails.AddAsync(orderDetail);
                }
                await _context.SaveChangesAsync();

                return "Đơn hàng momo tạm đã được tạo thành công!";
            }
            catch (DbUpdateException ex)
            {
                // Log detailed error message
                Log.Logger.Information(ex, "Database update exception occurred while creating order.");
                throw new Exception("Có lỗi xảy ra khi thêm vào bảng tạm momo. Chi tiết: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                // Log the error message
                Log.Logger.Information(ex, "General error occurred while creating order.");
                throw new Exception("Có lỗi xảy ra khi thêm vào bảng tạm momo.");
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
        public async Task<IEnumerable<object>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.Account) // Bao gồm thông tin tài khoản
                .Include(o => o.Address) // Bao gồm thông tin địa chỉ
                .Select(o => new
                {
                    o.Id,
                    o.code_order,
                    o.order_date,
                    o.TotalPrice,
                    o.Status_order,
                    o.PaymentID,
                    AccountName = o.Account != null ? o.Account.UserName : "N/A", // Kiểm tra tránh null
                    AddressName = o.Address != null ? o.Address.addressName : "N/A" // Kiểm tra tránh null
                })
                .ToListAsync();
        }

        public async Task<object> GetOrderDetailsByOrderIDPrintInvoice(int orderID)
        {
            var order = await (from o in _context.Orders
                               join a in _context.Accounts on o.accountID equals a.accountID into account
                               from acc in account.DefaultIfEmpty()
                               join ad in _context.Addresss on o.addressID equals ad.addressID into address
                               from addr in address.DefaultIfEmpty()
                               where o.Id == orderID
                               select new
                               {
                                   o.Id,
                                   o.code_order,
                                   o.order_date,
                                   o.TotalPrice,
                                   o.Status_order,
                                   PaymentMethod = o.PaymentID == 1 ? "Thanh toán khi nhận hàng" :
                                                   o.PaymentID == 2 ? "MoMo" : "VNPay",
                                   CustomerName = acc.UserName ?? "N/A",
                                   CustomerPhone = acc.Phone ?? "N/A",
                                   CustomerAddress = "Địa chỉ " + addr.addressName + " Thành phố " + addr.city + " Mã bưu điện " + addr.zipCode ?? "N/A",
                                   OrderDetails = (from od in o.OrderDetails
                                                   join p in _context.Products on od.ProductID equals p.Id
                                                   join c in _context.Categories on p.CategoryId equals c.Id
                                                   join t in _context.Types on p.TypeId equals t.Id
                                                   join s in _context.Suppliers on p.SupplierId equals s.Id
                                                   select new
                                                   {
                                                       p.Name,
                                                       p.Description,
                                                       Category = c.Name,
                                                       Type = t.Name,
                                                       Supplier = s.Name,
                                                       Img = p.ImageUrl
                                                   }).ToList()
                               }).FirstOrDefaultAsync();

            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
            }

            return new
            {
                OrderInfo = new
                {
                    order.Id,
                    order.code_order,
                    order.order_date,
                    order.TotalPrice,
                    order.Status_order,
                    order.PaymentMethod,
                    order.CustomerName,
                    order.CustomerPhone,
                    order.CustomerAddress
                },
                OrderDetails = order.OrderDetails
            };
        }

        //lấy sp theo id
        public async Task<OrderModel> GetOrderById(int Id)
        {
            return await _context.Orders.FindAsync(Id);
        }
        //lấy đơn hàng momo theo code_order
        public async Task<MomoDTO> GetMomoBycode_orderId(string code_order)
        {
            return await _context.Momos.FirstOrDefaultAsync(o => o.code_order == code_order);
        }
        //lấy đơn hàng chi tiết momo theo code_order
        public async Task<List<MomodetailDTO>> GetMomoDetailBycode_order(string code_order)
        {
            return await _context.MomoDetails
                        .Where(detail => detail.code_order == code_order)
                        .ToListAsync();
        }
        //lấy đơn hàng theo id người dùng
        public async Task<List<OrderModel>> GetOrdersByAccountID(int accountID)
        {
            return await _context.Orders
                                 .Where(p => p.accountID == accountID)
                                 .Include(detail => detail.Address)
                                 .ToListAsync();
        }

        //lấy đơn hàng chi tiết theo orderID
        public async Task<List<OrderDetailModel>> GetOrderDetailsByOrderID(int Id)
        {
            return await _context.OrderDetails
                                 .Where(p => p.orderID == Id)
                                 .Include(detail => detail.Product) // Join để lấy thông tin sản phẩm
                                 .ToListAsync();
        }
        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<Result> UpdateOrderAddressAsync(int orderID, int accountID, int addressID)
        {
            try
            {
                // Kiểm tra xem đơn hàng có tồn tại không
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderID && o.accountID == accountID);
                if (order == null)
                {
                    return new Result { Success = false, Message = "Đơn hàng không tồn tại hoặc không thuộc người dùng." };
                }

                // Cập nhật địa chỉ cho đơn hàng
                order.addressID = addressID;
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công với dữ liệu đơn hàng đã cập nhật
                return new Result
                {
                    Success = true,
                    Message = "Cập nhật địa chỉ thành công.",
                    Data = order // Trả về đơn hàng đã được cập nhật
                };
            }
            catch (Exception ex)
            {
                return new Result { Success = false, Message = "Lỗi hệ thống: " + ex.Message };
            }
        }

        public async Task UpdateOrderStatusAsync(int orderID, int status_order)
        {
            // Kiểm tra xem đơn hàng có tồn tại không
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderID);
            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại");
            }
            // Cập nhật địa chỉ cho đơn hàng
            order.Status_order = status_order;
            await _context.SaveChangesAsync();
        }

    }
}
