using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.ViewModel
{
    public class AddToCartRequest
    {
        public int AccountID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
