using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.ViewModel
{
    public class OrderItemRequest
    {
        public int productID { get; set; } // Đảm bảo thuộc tính này là public
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
