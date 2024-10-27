using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.AuthLogin
{
    public interface IAuthService
    {
        AccountModel Authenticate(string username, string password);

    }
}
