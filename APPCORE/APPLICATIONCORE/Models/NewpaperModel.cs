using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace APPLICATIONCORE.Models
{

    public class NewpaperModel
    {
        [Key]
        public int newpaperID { get; set; }

        [Required(ErrorMessage = "Tiêu đề bài báo là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tiêu đề bài báo phải chứa ít nhất 3 ký tự")]
        public string? NewpaperTitle { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Nội dung bài báo hoa phải chứa ít nhất 3 ký tự")]
        public string? Mota { get; set; }

        public string? ImageUrl { get; set; }
    }
}
