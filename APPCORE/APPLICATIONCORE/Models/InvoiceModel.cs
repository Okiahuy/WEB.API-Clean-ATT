
using System.ComponentModel.DataAnnotations;

namespace APPLICATIONCORE.Models
{

    public class InvoiceModel
    {
        [Key]
        public int invoiceID { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Payment { get; set; }

        public string? Address { get; set; }

    }
}
