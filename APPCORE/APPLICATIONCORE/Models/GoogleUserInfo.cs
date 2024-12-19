using APPLICATIONCORE.Models.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models
{
    public class GoogleUserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public int accountID { get; set; }
       
    }
}
