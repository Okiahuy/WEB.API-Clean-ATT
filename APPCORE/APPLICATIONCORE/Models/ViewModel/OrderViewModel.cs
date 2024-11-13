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
        public int? accountID;
        public int? paymentID;
        public int? productID;
        public decimal? TotalPrice;
        public int? Quantity;
        public double? Price;

        public string? Email; // Email của người dùng

        public List<OrderItemRequest> items;
    }
}
