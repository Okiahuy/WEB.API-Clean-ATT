using APPLICATIONCORE.Interface;
using APPLICATIONCORE.Interface.Account;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using INFRASTRUCTURE.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly MyDbContext _context;

        public AccountService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Accounts.CountAsync();
        }
        public async Task Register(AccountModel account)
        {
            var exsitingUsername = _context.Accounts.FirstOrDefault(x => x.UserName == account.UserName);
            if (exsitingUsername != null)
            {
                throw new ArgumentException("Tên đăng nhập đã tồn tại!");
            }
            var exsitingEmail = _context.Accounts.FirstOrDefault(x => x.Email == account.Email);
            if (exsitingEmail != null)
            {
                throw new ArgumentException("Email đã tồn tại!");
            }

            var hashedPassword = PasswordHelper.HashPassword(account.Password);

            var acc = new AccountModel
            {
                FullName = account.FullName,
                UserName = account.UserName,
                Email = account.Email,
                roleID = 2,
                Phone = account.Phone,
                level_cus = 0,
                Password = hashedPassword,
                Password2 = hashedPassword,
                isActive = 1
            };

            _context.Accounts.Add(acc);
            await _context.SaveChangesAsync();
        }

        //Phương thức hiển thị tài khoản theo accountID
        public async Task<List<AccountModel>> GetAccountByAccountIDAsync(int accountID)
        {
            var acc = await _context.Accounts
                .Where(c => c.accountID == accountID)
                .ToListAsync();
            return acc;
        }
        //Phương thức hiển thị tất cả tin nhắn
        public async Task<List<Notification>> GetNotificationsAsync()
        {
            var acc = await _context.Notis
                .Where(c => c.accountID == null)
                .OrderBy(n => n.notiID)
                .ToListAsync();
            return acc;
        }

        //Phương thức hiển thị tất cả tin tức
        public async Task<List<NewpaperModel>> GetNewpaperAsync()
        {
            var Newpapers = await _context.Newpapers
                .OrderBy(n => n.newpaperID)
                .ToListAsync();
            return Newpapers;
        }

        //Phương thức hiển thị tin nhắn theo accountID
        public async Task<List<Notification>> GetNotificationByAccountIDAsync(int accountID)
        {
        var acc = await _context.Notis
            .Where(c => c.accountID == accountID)
            .ToListAsync();
        return acc;
        }


    }
}

