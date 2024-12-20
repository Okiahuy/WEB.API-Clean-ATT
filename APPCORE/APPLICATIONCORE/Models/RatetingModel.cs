using System.ComponentModel.DataAnnotations;
//bảng đánh giá
namespace APPLICATIONCORE.Models
{
    public class RatetingModel
    {
        [Key]
        public int rateId { get; set; }

        public int? poductID { get; set; } // Khóa ngoại để liên kết với Sản phẩm
        public ProductModel? Product { get; set; }

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

        public string? Description { get; set; }
        public int? startnum { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}
