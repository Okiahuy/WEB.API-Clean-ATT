using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATIONCORE.History;
using APPLICATIONCORE.Models;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repository
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; } // cây
        public DbSet<ProductHistory> ProductHistory { get; set; } // cây lịch sử
        public DbSet<CategoryModel> Categories { get; set; } // loại cây3
        public DbSet<CartModel> Carts { get; set; } // giỏ hàng4
        public DbSet<OrderModel> Orders { get; set; } // đơn hàng5
        public DbSet<OrderDetailModel> OrderDetails { get; set; } //chi tiết đơn hàng6
        public DbSet<InvoiceModel> Invoices { get; set; } // lưu hóa đơn đã in
        public DbSet<AddressModel> Addresss { get; set; } //địa chỉ9
        public DbSet<AccountModel> Accounts { get; set; } // admin10
        public DbSet<RatetingModel> Rates { get; set; } // đánh giá11
        public DbSet<SupplierModel> Suppliers { get; set; } // Nhà cung cấp 12
        public DbSet<NewpaperModel> Newpapers { get; set; } //bài báo13
        public DbSet<FavouriteModel> Favourites { get; set; } // yêu thích 14
        public DbSet<RoleModel> Roles { get; set; } //phân quyền 15
        public DbSet<AnswerModel> Answers { get; set; } // trả lời 16
        public DbSet<TypeModel> Types { get; set; } // Loại hoa 17

		public DbSet<ApiUsageLog> ApiUsageLogs { get; set; }

	}
}
