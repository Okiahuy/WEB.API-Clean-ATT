using System.ComponentModel.DataAnnotations;

namespace APPLICATIONCORE.Models
{

    public class OrderDetailModel
    {
        [Key]
        public int orderdetailID { get; set; }
        public int orderID { get; set; } // Khóa ngoại để liên kết với Đơn hàng
        public OrderModel? Order { get; set; }

        public int? ProductID { get; set; } // Khóa ngoại để liên kết với Sản phẩm
        public ProductModel? Product { get; set; }

        public int Quantity { get; set; } // Số lượng sản phẩm trong đơn hàng

        public decimal Price { get; set; } // Giá của mỗi sản phẩm

        public int Status_order { get; set; }// 0 mới , 1 đang xử lý, 2 đang giao, 3 đã nhận 

    }
}
