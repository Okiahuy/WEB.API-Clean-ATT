using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APPLICATIONCORE.Models.Validation;
using Microsoft.AspNetCore.Http;


namespace APPLICATIONCORE.Models
{
    public class AccountModel
    {
        [Key]
        public int accountID { get; set; }

        [Required(ErrorMessage = "Tên khách bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên khách phải chứa ít nhất 3 ký tự")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên đăng nhập phải chứa ít nhất 3 ký tự")]
        public string? UserName { get; set; }

        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [MinLength(5, ErrorMessage = "Mật khẩu phải chứa ít nhất 5 ký tự")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        [DataType(DataType.Password)]
        public string? Password2 { get; set; }

        public decimal? level_cus { get; set; } ///cấp bậc của khách hàng 

        public DateTime Create { get; private set; }

        public AccountModel()
        {
            Create = DateTime.Now; // lần tạo đàu tiên
        }

        public string? ImageUrl { get; set; }

        [NotMapped] // Để không lưu trường này vào cơ sở dữ liệu
        [Required(ErrorMessage = "Ảnh về hoa là bắt buộc")]
        [FileExtension] //  kiểm tra loại file
        public IFormFile? ImageUpload { get; set; }

        public DateTime? LastLogin { get; set; } = DateTime.Now;

        public int? roleID { get; set; } // Khóa ngoại để liên kết 

        [ForeignKey("roleID")]
        public RoleModel? Role { get; set; }

        public int? isActive { get; set; }
    }
}
