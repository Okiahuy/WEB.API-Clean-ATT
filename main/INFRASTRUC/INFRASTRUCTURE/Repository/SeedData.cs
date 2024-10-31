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
            if (!context.Roles.Any() || !context.Accounts.Any() || !context.Categories.Any() || !context.Suppliers.Any() || !context.Types.Any() || !context.Products.Any())
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
                    new CategoryModel { Name = "Hoa Tình Yêu", Description = "Mô tả cho Hoa Tình Yêu" },
                    new CategoryModel { Name = "Hoa Chia Buồn", Description = "Mô tả cho Hoa Chia Buồn" },
                    new CategoryModel { Name = "Hoa Mừng Tuổi", Description = "Mô tả cho Hoa Mừng Tuổi" },
                    new CategoryModel { Name = "Hoa Tình Bạn", Description = "Mô tả cho Hoa Tình Bạn" },
                    new CategoryModel { Name = "Hoa Kiểng", Description = "Mô tả cho Hoa Tình Bạn" }

                );
                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                context.Suppliers.AddRange(
                    new SupplierModel { Name = "Hoa Channel", address = "Chợ Tân Việt Hòa" },
                    new SupplierModel { Name = "Hoa Studio", address = "Chợ Cao Lãnh" },
                    new SupplierModel { Name = "Tân Việt Hòa Market", address = "Chợ Tân Việt Hòa phường 6 thành phố cao lãnh!" },
                    new SupplierModel { Name = "Hội Khuyến Học", address = "Thành Phố Hồ Chí Minh" }
                );
                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                context.Types.AddRange(
                    new TypeModel { Name = "Hoa Thật", Description = "Mô tả cho hoa" },
                    new TypeModel { Name = "Hoa Giấy", Description = "Mô tả cho hoa" },
                    new TypeModel { Name = "Hoa Sáp", Description = "Mô tả cho hoa" },
                    new TypeModel { Name = "Hoa Nhựa", Description = "Mô tả cho hoa" },
                    new TypeModel { Name = "Hoa Thân Gỗ", Description = "Mô tả cho hoa" }

                );
                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                string[] names = {
                    "Hoa Tình Yêu Đôi Lứa", "Hoa Hồng Đỏ Rực", "Hoa Mẫu Đơn Kiêu Sa", "Hoa Sen Tinh Khiết",
                    "Hoa Cẩm Chướng Đầy Sắc", "Hoa Tulip Dịu Dàng", "Hoa Bách Hợp Trắng", "Hoa Ly Hồng Phấn",
                    "Hoa Baby Ngọt Ngào", "Hoa Lan Hồ Điệp Tinh Tế", "Hoa Hướng Dương Rạng Rỡ",
                    "Hoa Anh Đào Nhật Bản", "Hoa Cúc Vàng Tươi Sáng", "Hoa Phong Lan Độc Đáo",
                    "Hoa Thủy Tiên Thanh Thoát", "Hoa Cúc Họa Mi Dễ Thương", "Hoa Lưu Ly Kỷ Niệm",
                    "Hoa Lan Cattleya Sang Trọng"
                };

                string[] descriptions = {
                    "Hoa thích hợp cho các buổi hẹn hò lãng mạn.", "Hoa giành tặng cho sinh nhật người thân yêu.",
                    "Hoa đẹp để chúc mừng những dịp đặc biệt.", "Hoa biểu tượng cho tình yêu và sự trong sáng.",
                    "Hoa này phù hợp làm quà tặng dịp lễ.", "Hoa mang đến cảm giác bình yên và thanh tịnh.",
                    "Hoa sang trọng cho sự kiện quan trọng.", "Hoa dành cho các buổi gặp mặt thân mật.",
                    "Hoa tạo niềm vui cho những khoảnh khắc đặc biệt.", "Hoa đem lại sự tươi mới và sức sống.",
                    "Hoa tràn đầy sức sống cho dịp kỷ niệm.", "Hoa gợi lên cảm giác tươi vui và trẻ trung.",
                    "Hoa là món quà dễ thương và ý nghĩa.", "Hoa tượng trưng cho sự yêu thương mãnh liệt.",
                    "Hoa nhẹ nhàng, tinh tế cho người đặc biệt.", "Hoa đẹp phù hợp trang trí phòng khách.",
                    "Hoa mang vẻ đẹp dịu dàng cho buổi tối lãng mạn.", "Hoa thanh lịch dành cho dịp quan trọng."
                };

                List<ProductModel> products = new List<ProductModel>();

                for (int i = 0; i <= 17; i++)
                {
                    string img = $"/uploads/{i + 1}.jpg";
                    products.Add(new ProductModel
                    {
                        Name = names[i % names.Length],
                        Description = descriptions[i % descriptions.Length],
                        SupplierId = 1,  
                        TypeId = 2,      
                        CategoryId = 2,  
                        Price = 200000 + (i * 10000), 
                        DisPrice = i % 5 + 1,
                        CreatedByRoleID = 1,
                        Quantity = 100 - i, 
                        ImageUrl = img
                    });
                }

                context.Products.AddRange(products);
                context.SaveChanges();

            }


        }
    }
}
