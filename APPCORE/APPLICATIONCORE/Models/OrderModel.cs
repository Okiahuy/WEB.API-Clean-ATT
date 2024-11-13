using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APPLICATIONCORE.Models
{

    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? code_order { get; set; }

        public DateTime? order_date { get; set; } = DateTime.Now;

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

        public decimal TotalPrice { get; set; } // Tổng giá trị của từng mục trong đơn hàng
        [JsonIgnore]
        public List<OrderDetailModel>? OrderDetails { get; set; } // Danh sách các chi tiết đơn hàng

        public int Status_order { get; set; }// 0 mới , 1 đang xử lý, 2 đang giao, 3 đã nhận 

        public int PaymentID { get; set; }
    }
}
