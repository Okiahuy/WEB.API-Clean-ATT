using APPLICATIONCORE.Models;
using APPLICATIONCORE.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Domain.Momo.MomoDtos
{
    public class MomodetailDTO
    {
        [Key]
        public int momodetailID { get; set; }
        public int orderID { get; set; } // Khóa ngoại để liên kết với Đơn hàng
        public string? code_order { get; set; }
        public int? ProductID { get; set; } // Khóa ngoại để liên kết với Sản phẩm

        public int Quantity { get; set; } // Số lượng sản phẩm trong đơn hàng

        public decimal Price { get; set; } // Giá của mỗi sản phẩm

       
    }
}
