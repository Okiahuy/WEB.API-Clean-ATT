using APPLICATIONCORE.Interface.AuthLogin;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace INFRASTRUCTURE.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly MyDbContext _context;

        public AuthService(MyDbContext context)
        {
            _context = context;
        }

        public AccountModel Authenticate(string username, string password)
        { 
             // Mã hóa mật khẩu người dùng nhập vào
            var hashedPassword = PasswordHelper.HashPassword(password);
            // Tìm kiếm người dùng trong cơ sở dữ liệu
            var account = _context.Accounts.FirstOrDefault(a => a.UserName == username && a.Password == hashedPassword);

            if (account == null)
            {
                return null; // Nếu không tìm thấy người dùng
            }

            return account; // Trả về thông tin người dùng
        }

        
    }
}
