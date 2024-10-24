using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using APPLICATIONCORE.Models.Validation;


namespace APPLICATIONCORE.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hoa là bắt buộc")]
        [MinLength(5, ErrorMessage = "Tên hoa phải lớn hơn 5 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Mô tả về hoa là bắt buộc")]
        [MinLength(5, ErrorMessage = "Mô tả về hoa phải lớn hơn 5 ký tự")]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal DisPrice { get; set; }

        [Required(ErrorMessage = "Số lượng về hoa là bắt buộc")]
        public long? Quantity { get; set; } // số lượng sản phẩm

        public long? likecount { get; set; } = 0;// số lượng người yêu thích sản phẩm này 

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryModel? Category { get; set; }

        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public SupplierModel? supplier { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public TypeModel? type { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped] // Để không lưu trường này vào cơ sở dữ liệu
        [Required(ErrorMessage = "Ảnh về hoa là bắt buộc")]
        [FileExtension] //  kiểm tra loại file
        public IFormFile? ImageUpload { get; set; }





    }
}

