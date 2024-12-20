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
            return await _context.Accounts.Where(p => p.roleID == 2).CountAsync();
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
                isActive = 1,
                ImageUrl = "New Avatar"
            };

            _context.Accounts.Add(acc);
            await _context.SaveChangesAsync();
        }
        private void DeleteOldFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
        //hiển thị câu hỏi theo productID
        public async Task<IEnumerable<AnswerModel>> GetAnswersByProductIdAsync(int productId)
        {
            return await _context.Answers
                .Where(answer => answer.productID == productId)
                .Include(answer => answer.Account) // Nếu cần thêm thông tin tài khoản
                .ToListAsync();
        }

        public async Task AddAnswerAsync(int accountID, int productID, string descriptionAnswer, string fullnameAnswer, string emailAnswer)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(descriptionAnswer))
            {
                throw new ArgumentException("Bình luận không được để trống.", nameof(descriptionAnswer));
            }

            if (string.IsNullOrWhiteSpace(fullnameAnswer))
            {
                throw new ArgumentException("Họ & Tên không được để trống.", nameof(fullnameAnswer));
            }

            if (string.IsNullOrWhiteSpace(emailAnswer))
            {
                throw new ArgumentException("Email không được để trống.", nameof(emailAnswer));
            }

            // Tạo mới đối tượng AnswerModel
            var answer = new AnswerModel
            {
                accountID = accountID,
                productID = productID,
                DescriptionAnswer = descriptionAnswer,
                fullnameAnswer = fullnameAnswer,
                emailAnswer = emailAnswer,
                Date = DateTime.Now
            };

            // Thêm đối tượng vào cơ sở dữ liệu
            await _context.Answers.AddAsync(answer);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        //hàm cập nhật thông tin người dùng
        public async Task<AccountModel> UpdateaccountAsync(int accountID, AccountModel account)
        {
            // Tìm người dùng theo ID
            var existingaccount = await _context.Accounts.FindAsync(accountID);
            if (existingaccount == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng để sửa!");
            }
            // Cập nhật các trường khác nếu được gửi
            if (!string.IsNullOrEmpty(account.FullName))
            {
                existingaccount.FullName = account.FullName;
            }    
            if (!string.IsNullOrEmpty(account.Phone))
            {
                existingaccount.Phone = account.Phone;
            }    
            if (!string.IsNullOrEmpty(account.Password))
            {
                var hashedPassword = PasswordHelper.HashPassword(account.Password);
                existingaccount.Password = hashedPassword;
                existingaccount.Password2 = hashedPassword;
            }
            if (!string.IsNullOrEmpty(account.Email))
            {
                existingaccount.Email = account.Email;
            }
            if (!string.IsNullOrEmpty(account.UserName))
            {
                existingaccount.UserName = existingaccount.UserName;
            }
            // Lấy đường dẫn của ảnh cũ
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(existingaccount.ImageUrl));
           
           
            // Kiểm tra nếu có ảnh mới được tải lên
            if (account.ImageUpload != null)
            {
                // Tạo tên file với timestamp để tránh trùng lặp
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{Path.GetFileNameWithoutExtension(account.ImageUpload.FileName)}_{timeStamp}{Path.GetExtension(account.ImageUpload.FileName)}";
                var imagePath = Path.Combine("uploads", fileName);

                // Lưu file ảnh mới
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await account.ImageUpload.CopyToAsync(stream);
                }
                // Cập nhật đường dẫn ảnh mới vào người dùng
                existingaccount.ImageUrl = $"/uploads/{fileName}";
                // Xóa ảnh cũ (nếu có)
                DeleteOldFile(oldImagePath);
            }
           

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingaccount;
        }

        //Phương thức hiển thị tài khoản người dùng
        public async Task<List<AccountModel>> GetAccountByRoleIDAsync()
        {
            var acc = await _context.Accounts
                .Where(c => c.roleID == 2)
                .ToListAsync();
            return acc;
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

