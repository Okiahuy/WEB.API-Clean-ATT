using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.Validation
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("Bạn không có quyền truy cập vào tài nguyên này.") { }

        public ForbiddenAccessException(string message) : base(message) { }

        public ForbiddenAccessException(string message, Exception inner) : base(message, inner) { }

  

    }
}
