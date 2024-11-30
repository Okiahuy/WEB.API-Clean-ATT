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
            try
            {
                var product = await _context.Products.FindAsync(productID);
                if (product == null)
                {
                    throw new Exception("Sản phẩm không tồn tại");
                }
                // Kiểm tra sự tồn tại của tài khoản
                var account = await _context.Accounts.FindAsync(accountID);
                if (account == null)
                {
                    throw new Exception("Tài khoản không tồn tại");
                }
                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.accountID == accountID && c.productID == productID);

                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                    cartItem.TotalPrice = cartItem.Quantity * product.Price;
                }
                else
                {
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

                // Log thông tin trước khi lưu thay đổi
                Console.WriteLine($"Saving cart item: AccountID={accountID}, ProductID={productID}, Quantity={quantity}");

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }


        //Phương thức cập nhật số lượng sản phẩm trong giỏ hàng
        public async Task UpdateCartItemQuantityAsync(int accountID, int productID, int newQuantity)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.accountID == accountID && c.productID == productID);

            if (cartItem == null)
            {
                throw new Exception("Sản phẩm không tồn tại trong giỏ hàng.");
            }

            if (newQuantity <= 0)
            {
                throw new Exception("Số lượng sản phẩm phải lớn hơn 0.");
            }

            cartItem.Quantity = newQuantity;
            cartItem.TotalPrice = cartItem.Quantity * cartItem.Price;

            await _context.SaveChangesAsync();
        }
        //Phương thức xóa sản phẩm khỏi giỏ hàng
        public async Task RemoveFromCartAsync(int accountID, int productID)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.accountID == accountID && c.productID == productID && c.Status_cart == 0);

            if (cartItem == null)
            {
                throw new Exception("Sản phẩm không tồn tại trong giỏ hàng.");
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }


        //Phương thức hiển thị giỏ hàng theo accountID
        public async Task<List<CartModel>> GetCartByAccountIDAsync(int accountID)
        {
            var cartItems = await _context.Carts
                .Where(c => c.accountID == accountID)
                .ToListAsync();

            if (!cartItems.Any())
            {
                throw new Exception("Giỏ hàng hiện đang trống.");
            }

            return cartItems;
        }

    }
}
