using System.ComponentModel.DataAnnotations;
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
        [JsonIgnore]
        public ICollection<ProductModel>? Products { get; set; }
    }
}
