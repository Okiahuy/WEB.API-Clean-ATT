using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Domain.Momo.MomoDtos
{
    public class MomoDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? code_order { get; set; }

        public DateTime? order_date { get; set; } = DateTime.Now;

        public int accountID { get; set; } 

        public decimal TotalPrice { get; set; } 
        [JsonIgnore]
        public List<MomodetailDTO>? MomoDetails { get; set; } 

        public int PaymentID { get; set; }//1 thanh toán khi nhận hàng, 2 thanh toán vnpay, 3 thanh toán momo

        public string? PaymentName { get; set; }

        public string? email { get; set; }
        public int addressID { get; set; }
    }
}
