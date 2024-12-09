using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUsageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestCount = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpuUsage = table.Column<double>(type: "float", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsageLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Momos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code_order = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    PaymentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Momos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Newpapers",
                columns: table => new
                {
                    newpaperID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewpaperTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newpapers", x => x.newpaperID);
                });

            migrationBuilder.CreateTable(
                name: "ProductHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByRoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MomoDetails",
                columns: table => new
                {
                    momodetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderID = table.Column<int>(type: "int", nullable: false),
                    code_order = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MomoDTOId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MomoDetails", x => x.momodetailID);
                    table.ForeignKey(
                        name: "FK_MomoDetails_Momos_MomoDTOId",
                        column: x => x.MomoDTOId,
                        principalTable: "Momos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    accountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    level_cus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    roleID = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.accountID);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_roleID",
                        column: x => x.roleID,
                        principalTable: "Roles",
                        principalColumn: "roleID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    likecount = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    typeProduct = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByRoleID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByRoleID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresss",
                columns: table => new
                {
                    addressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    addressName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresss", x => x.addressID);
                    table.ForeignKey(
                        name: "FK_Addresss_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    invoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    accountID = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.invoiceID);
                    table.ForeignKey(
                        name: "FK_Invoices_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                });

            migrationBuilder.CreateTable(
                name: "Notis",
                columns: table => new
                {
                    notiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notis", x => x.notiID);
                    table.ForeignKey(
                        name: "FK_Notis_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code_order = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status_order = table.Column<int>(type: "int", nullable: false),
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountID = table.Column<int>(type: "int", nullable: true),
                    productID = table.Column<int>(type: "int", nullable: true),
                    DescriptionAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fullnameAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerID);
                    table.ForeignKey(
                        name: "FK_Answers_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                    table.ForeignKey(
                        name: "FK_Answers_Products_productID",
                        column: x => x.productID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    cartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productID = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status_cart = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.cartID);
                    table.ForeignKey(
                        name: "FK_Carts_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                    table.ForeignKey(
                        name: "FK_Carts_Products_productID",
                        column: x => x.productID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    favouriteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountID = table.Column<int>(type: "int", nullable: true),
                    productID = table.Column<int>(type: "int", nullable: true),
                    count = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.favouriteID);
                    table.ForeignKey(
                        name: "FK_Favourites_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                    table.ForeignKey(
                        name: "FK_Favourites_Products_productID",
                        column: x => x.productID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    ratetingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    poductID = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startnum = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.ratetingId);
                    table.ForeignKey(
                        name: "FK_Rates_Accounts_accountID",
                        column: x => x.accountID,
                        principalTable: "Accounts",
                        principalColumn: "accountID");
                    table.ForeignKey(
                        name: "FK_Rates_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    orderdetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.orderdetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_orderID",
                        column: x => x.orderID,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_roleID",
                table: "Accounts",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_accountID",
                table: "Addresss",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_accountID",
                table: "Answers",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_productID",
                table: "Answers",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_accountID",
                table: "Carts",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_productID",
                table: "Carts",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_accountID",
                table: "Favourites",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_productID",
                table: "Favourites",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_accountID",
                table: "Invoices",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_MomoDetails_MomoDTOId",
                table: "MomoDetails",
                column: "MomoDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Notis_accountID",
                table: "Notis",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_orderID",
                table: "OrderDetails",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_accountID",
                table: "Orders",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_accountID",
                table: "Rates",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ProductId",
                table: "Rates",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresss");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "ApiUsageLogs");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MomoDetails");

            migrationBuilder.DropTable(
                name: "Newpapers");

            migrationBuilder.DropTable(
                name: "Notis");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductHistory");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "Momos");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
