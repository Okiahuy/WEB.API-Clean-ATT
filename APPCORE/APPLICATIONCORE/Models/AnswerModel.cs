using System.ComponentModel.DataAnnotations;
///bảng câu trả lời
namespace APPLICATIONCORE.Models
{

    public class AnswerModel
    {
        [Key]
        public int AnswerID { get; set; }

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

        public int? productID { get; set; }
        public ProductModel? Product { get; set; }

        [Required(ErrorMessage = "Nhập bình luận của bạn, bình luận không được để trống!")]
        public string? DescriptionAnswer { get; set; }
        [Required(ErrorMessage = "Họ & Tên không được để trống!")]
        public string? fullnameAnswer { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        public string? emailAnswer { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
