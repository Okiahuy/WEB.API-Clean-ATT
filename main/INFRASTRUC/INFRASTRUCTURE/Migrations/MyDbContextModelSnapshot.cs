﻿// <auto-generated />
using System;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APPLICATIONCORE.History.ProductHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedByRoleID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ProductHistory");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.AccountModel", b =>
                {
                    b.Property<int>("accountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("accountID"));

                    b.Property<DateTime>("Create")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("isActive")
                        .HasColumnType("int");

                    b.Property<decimal?>("level_cus")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("roleID")
                        .HasColumnType("int");

                    b.HasKey("accountID");

                    b.HasIndex("roleID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.AddressModel", b =>
                {
                    b.Property<int>("addressID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("addressID"));

                    b.Property<DateTime?>("Create")
                        .HasColumnType("datetime2");

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.Property<string>("addressName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("zipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("addressID");

                    b.HasIndex("accountID");

                    b.ToTable("Addresss");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.AnswerModel", b =>
                {
                    b.Property<int>("AnswerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnswerID"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DescriptionAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.Property<string>("emailAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullnameAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("productID")
                        .HasColumnType("int");

                    b.HasKey("AnswerID");

                    b.HasIndex("accountID");

                    b.HasIndex("productID");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.ApiUsageLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApiName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CpuUsage")
                        .HasColumnType("float");

                    b.Property<int?>("RequestCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApiUsageLogs");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.CartModel", b =>
                {
                    b.Property<int>("cartID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("cartID"));

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Status_cart")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.Property<int?>("productID")
                        .HasColumnType("int");

                    b.HasKey("cartID");

                    b.HasIndex("accountID");

                    b.HasIndex("productID");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.CategoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.FavouriteModel", b =>
                {
                    b.Property<int>("favouriteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("favouriteID"));

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.Property<long?>("count")
                        .HasColumnType("bigint");

                    b.Property<int?>("productID")
                        .HasColumnType("int");

                    b.HasKey("favouriteID");

                    b.HasIndex("accountID");

                    b.HasIndex("productID");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.InvoiceModel", b =>
                {
                    b.Property<int>("invoiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("invoiceID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Payment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.HasKey("invoiceID");

                    b.HasIndex("accountID");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.NewpaperModel", b =>
                {
                    b.Property<int>("newpaperID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("newpaperID"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mota")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewpaperTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("newpaperID");

                    b.ToTable("Newpapers");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.OrderDetailModel", b =>
                {
                    b.Property<int>("orderdetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("orderdetailID"));

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Status_order")
                        .HasColumnType("int");

                    b.Property<int>("orderID")
                        .HasColumnType("int");

                    b.HasKey("orderdetailID");

                    b.HasIndex("ProductID");

                    b.HasIndex("orderID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.OrderModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PaymentID")
                        .HasColumnType("int");

                    b.Property<int>("Status_order")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.Property<string>("code_order")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("order_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("accountID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.ProductModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedByRoleID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DisPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("Quantity")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedByRoleID")
                        .HasColumnType("int");

                    b.Property<long?>("likecount")
                        .HasColumnType("bigint");

                    b.Property<int?>("typeProduct")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("TypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.RatetingModel", b =>
                {
                    b.Property<int>("ratetingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ratetingId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("accountID")
                        .HasColumnType("int");

                    b.Property<int?>("poductID")
                        .HasColumnType("int");

                    b.Property<int?>("startnum")
                        .HasColumnType("int");

                    b.HasKey("ratetingId");

                    b.HasIndex("ProductId");

                    b.HasIndex("accountID");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.RoleModel", b =>
                {
                    b.Property<int>("roleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("roleID"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("roleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.SupplierModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.TypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.AccountModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.RoleModel", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("roleID");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.AddressModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.AnswerModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.HasOne("APPLICATIONCORE.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("productID");

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.CartModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.HasOne("APPLICATIONCORE.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("productID");

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.FavouriteModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.HasOne("APPLICATIONCORE.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("productID");

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.InvoiceModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.OrderDetailModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.HasOne("APPLICATIONCORE.Models.OrderModel", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("orderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.OrderModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.ProductModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.CategoryModel", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPLICATIONCORE.Models.SupplierModel", "supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APPLICATIONCORE.Models.TypeModel", "type")
                        .WithMany("Products")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("supplier");

                    b.Navigation("type");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.RatetingModel", b =>
                {
                    b.HasOne("APPLICATIONCORE.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("APPLICATIONCORE.Models.AccountModel", "Account")
                        .WithMany()
                        .HasForeignKey("accountID");

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.CategoryModel", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.OrderModel", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.RoleModel", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.SupplierModel", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("APPLICATIONCORE.Models.TypeModel", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
