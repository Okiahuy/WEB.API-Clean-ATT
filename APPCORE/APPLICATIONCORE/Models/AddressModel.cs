using System.ComponentModel.DataAnnotations;

namespace APPLICATIONCORE.Models
{

    public class AddressModel
    {
        [Key]
        public int addressID { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string? addressName { get; set; }
        [Required(ErrorMessage = "Thành phố không được để trống")]
        public string? city { get; set; }
        [Required(ErrorMessage = "Mã bưu điện không được để trống")]  
        public string? zipCode { get; set; }

        public DateTime? Create { get; set; } = DateTime.Now;

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

    }
}
