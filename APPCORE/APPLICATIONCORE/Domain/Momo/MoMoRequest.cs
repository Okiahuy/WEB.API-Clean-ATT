using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Domain.Momo
{
    public class MoMoRequest
    {
        public string partnerCode { get; set; }
        public string accessKey { get; set; }
        public string secretKey { get; set; }
        public string orderInfo { get; set; }
        public decimal amount { get; set; }
        public string orderId { get; set; }
        public string redirectUrl { get; set; }
        public string ipnUrl { get; set; }
    }
}
