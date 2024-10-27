using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATIONCORE.Models;


namespace INFRASTRUCTURE.Repository
{
    public class SeedData
    {
        public static void SeedDingData(MyDbContext context)
        {
            // Kiểm tra xem dữ liệu đã tồn tại trong bảng hay không
            context.Database.Migrate();
            if (!context.Roles.Any() || !context.Accounts.Any() || !context.Categories.Any() || !context.Suppliers.Any() || !context.Types.Any())
            {
                context.Roles.AddRange(
                       new RoleModel { RoleName = "Quản Chị" },
                       new RoleModel { RoleName = "Khách Hàng" }

                 );
                context.SaveChanges();
                // Tạo dữ liệu cho bảng admin
                context.Accounts.AddRange(

                    new AccountModel { FullName = "Oppa Web Bán Hoa", UserName = "admin", roleID = 1, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "huydzaa12@gmail.com", Phone = "0337899123" },
                    new AccountModel { FullName = "Nguyễn Văn A", UserName = "nguyenvana", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "nguyenvana@gmail.com", Phone = "0337899123" }

                );
                context.SaveChanges();

                context.Categories.AddRange(
                    new CategoryModel { Name = "Hoa Chúc Mừng", Description = "Mô tả cho Hoa Chúc Mừng" },
                    new CategoryModel { Name = "Hoa Tình Yêu", Description = "Mô tả cho Hoa Tình Yêu" }

                );
                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                context.Suppliers.AddRange(
                    new SupplierModel { Name = "Hoa Channel", address = "Chợ Tân Việt Hòa" },
                    new SupplierModel { Name = "Hoa Studio", address = "Chợ Cao Lãnh" }
                );
                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                context.Types.AddRange(
                    new TypeModel { Name = "Hoa Thật", Description = "Mô tả cho hoa" },
                    new TypeModel { Name = "Hoa Giấy", Description = "Mô tả cho hoa" }

                );
                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();
            }


        }
    }
}
