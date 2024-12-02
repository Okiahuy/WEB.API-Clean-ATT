using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.ViewModel
{
    public class OrderViewModel
    {
        public int accountID { get; set; }
        public int paymentID { get; set; }
        public decimal totalPrice { get; set; }
        public string email { get; set; } // Email của người dùng

        public List<OrderItemRequest> orderItemRequests { get; set; }
    }
}
