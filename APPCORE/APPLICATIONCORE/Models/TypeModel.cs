using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

//bảng loại hoa
namespace APPLICATIONCORE.Models
{
    public class TypeModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên loại phải lớn hơn 3 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [MinLength(5, ErrorMessage = "Mô tả phải lớn hơn 5 ký tự")]
        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<ProductModel>? Products { get; set; }

    }
}
