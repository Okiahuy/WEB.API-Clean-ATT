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
        public int ProductID { get; set; } // Đảm bảo thuộc tính này là public
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
