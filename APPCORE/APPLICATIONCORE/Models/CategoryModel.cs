using APPLICATIONCORE.Models.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
//bảng danh mục
namespace APPLICATIONCORE.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên danh mục phải lớn hơn 3 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [MinLength(5, ErrorMessage = "Mô tả phải lớn hơn 5 ký tự")]

      
        public string? Description { get; set; }
		public string? ImageUrl { get; set; }

		[NotMapped] // Để không lưu trường này vào cơ sở dữ liệu
		[FileExtension] //  kiểm tra loại file
		public IFormFile? ImageUpload { get; set; }


		[JsonIgnore]
        public ICollection<ProductModel>? Products { get; set; }
    }
}
