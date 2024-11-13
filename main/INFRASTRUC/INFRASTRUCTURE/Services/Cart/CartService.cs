using APPLICATIONCORE.Interface.Cart;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly MyDbContext _context;

        public CartService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int accountID, int productID, int quantity)
        {
            // Tìm sản phẩm trong cơ sở dữ liệu
            var product = await _context.Products.FindAsync(productID);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại");
            }

            // Tìm sản phẩm trong giỏ hàng của người dùng
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.accountID == accountID && c.productID == productID && c.Status_cart == 0);

            if (cartItem != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, cập nhật số lượng và tổng giá
                cartItem.Quantity += quantity;
                cartItem.TotalPrice = cartItem.Quantity * product.Price;
            }
            else
            {
                // Nếu chưa có, thêm sản phẩm mới vào giỏ hàng
                cartItem = new CartModel
                {
                    accountID = accountID,
                    productID = productID,
                    ProductName = product.Name,
                    Quantity = quantity,
                    Price = product.Price,
                    TotalPrice = quantity * product.Price,
                    Status_cart = 0
                };
                await _context.Carts.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
