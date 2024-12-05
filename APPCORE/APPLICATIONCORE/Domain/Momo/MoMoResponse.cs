using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Domain.Momo
{
    public class MoMoResponse
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string payUrl { get; set; }
    }
}
