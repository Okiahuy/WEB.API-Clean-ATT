using System.ComponentModel.DataAnnotations;
//bảng giỏ hàng
namespace APPLICATIONCORE.Models
{

    public class CartModel
    {
        [Key]
        public int cartID { get; set; }

        public int? productID { get; set; } // Khóa ngoại để liên kết với Sản phẩm
        public ProductModel? Product { get; set; }
        public string? ProductName { get; set; }

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public int Status_cart { get; set; } = 0; // 0 chư thanh toán, 1 đã thanh toán 
    }
}
