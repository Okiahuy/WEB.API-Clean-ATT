using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Domain.Momo
{
    public class MoMoNotifyResponse
    {
        public int resultCode { get; set; }
        public string orderId { get; set; }
        public string message { get; set; }
        public string signature { get; set; }
        // Các thông tin khác nếu cần
        public decimal amount { get; set; }
        public string requestId { get; set; }
        public string transId { get; set; }
    }
}
