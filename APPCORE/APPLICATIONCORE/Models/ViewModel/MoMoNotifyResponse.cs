using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.ViewModel
{
    public class MoMoNotifyResponse
    {
        public int resultCode { get; set; }
        public string orderId { get; set; }
        public string message { get; set; }
        // Các thông tin khác nếu cần
    }
}
