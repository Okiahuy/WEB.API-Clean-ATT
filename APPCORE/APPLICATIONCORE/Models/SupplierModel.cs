using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
///bảng nhà cung cấp
namespace APPLICATIONCORE.Models
{

    public class SupplierModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc")]
        [MinLength(5, ErrorMessage = "Tên nhà cung cấp phải lớn hơn 5 ký tự")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Địa chỉ nhà cung cấp là bắt buộc")]
        [MinLength(5, ErrorMessage = "Địa chỉ nhà cung cấp phải lớn hơn 5 ký tự")]
        public string? address { get; set; }
        [JsonIgnore]
        public ICollection<ProductModel>? Products { get; set; }
    }
}
