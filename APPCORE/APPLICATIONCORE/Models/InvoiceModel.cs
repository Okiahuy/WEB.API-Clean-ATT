
using System.ComponentModel.DataAnnotations;

namespace APPLICATIONCORE.Models
{

    public class InvoiceModel
    {
        [Key]
        public int invoiceID { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public int orderID { get; set; } // Khóa ngoại để liên kết với Đơn hàng
        public OrderModel? Order { get; set; }

        public string? CustomerName { get; set; } // Khóa ngoại để liên kết với Khách hàng

        public decimal TotalAmount { get; set; }

        public int Status_order { get; set; }// 0 mới , 1 đang xử lý, 2 đang giao, 3 đã nhận 

        public string? PaymentMethod { get; set; }

        public string? Address { get; set; }

    }
}
