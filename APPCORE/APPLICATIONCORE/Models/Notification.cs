using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models
{
    public class Notification
    {
        [Key]
        public int notiID { get; set; }

        [Required(ErrorMessage = "Nội dung thông báo không được để trống")]
        public string? description { get; set; }

        public DateTime? Create { get; set; } = DateTime.Now;

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }
    }
}
