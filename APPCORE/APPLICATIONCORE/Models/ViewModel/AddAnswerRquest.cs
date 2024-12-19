using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.ViewModel
{
    public class AddAnswerRquest
    {
        public int accountID { get; set; }
        public int productID { get; set; }
        public string DescriptionAnswer { get; set; }
        public string fullnameAnswer { get; set; }
        public string emailAnswer { get; set; }
    }
}
