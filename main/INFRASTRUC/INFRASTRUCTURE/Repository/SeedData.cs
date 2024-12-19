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
            if (!context.Roles.Any() || !context.Accounts.Any() || !context.Categories.Any() ||
                !context.Suppliers.Any() || !context.Types.Any() || !context.Products.Any() ||
                !context.Newpapers.Any() || !context.Notis.Any() || !context.Addresss.Any() )
            {
                context.Roles.AddRange(
                       new RoleModel { RoleName = "Quản Trị" },
                       new RoleModel { RoleName = "Khách Hàng" }
                 );
                context.SaveChanges();
                // Tạo dữ liệu cho bảng admin
                context.Accounts.AddRange(
                    new AccountModel { FullName = "Oppa Web Bán Hoa", UserName = "admin", roleID = 1, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "huydzaa12@gmail.com", Phone = "0337899123",level_cus = 0 },
                    new AccountModel { FullName = "Nguyễn Văn A", UserName = "nguyenvana", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "nguyenvana@gmail.com", Phone = "0337899124", level_cus = 0 },
                    new AccountModel { FullName = "Nguyễn Thị Kim Hân", UserName = "hankim", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "kimhan29072003@gmail.com", Phone = "0337899125", level_cus = 0 },
                    new AccountModel { FullName = "Huỳnh Anh Khoa", UserName = "khoanh", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "huynhanhkhoa30042019@gmail.com", Phone = "0337899126", level_cus = 0 },
                    new AccountModel { FullName = "Nguyễn Tấn Huy", UserName = "huynguyen", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "huynguyen@gmail.com", Phone = "0337899127", level_cus = 0 },
                    new AccountModel { FullName = "Trần Duy Đăng", UserName = "dangtran", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "tranduydang@gmail.com", Phone = "0337899128", level_cus = 0 },
                    new AccountModel { FullName = "Nguyễn Cẩm Châu", UserName = "camchau", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "camchau@gmail.com", Phone = "0337899129", level_cus = 0 },
                    new AccountModel { FullName = "Trần Công Dinh", UserName = "dinhtran", roleID = 2, Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", Password2 = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", isActive = 1, Email = "dinhtran@gmail.com", Phone = "0337899120", level_cus = 0 }
                );
                context.SaveChanges();
                //bảng địa chỉ
                context.Addresss.AddRange(
                      new AddressModel { addressName = "A1000 Phạm Hữu Lầu, Phường 6", city = "Cao Lãnh", Create = DateTime.Now, zipCode ="123212", accountID=2},
                      new AddressModel { addressName = "13300 Lý Thường Kiệt, Phường 2", city = "Hồ Chí Minh", Create= DateTime.Now, zipCode = "566546", accountID = 2 },
                      new AddressModel { addressName = "160 Xã Mỹ Trà", city = "Cao Lãnh", Create = DateTime.Now, zipCode = "12345", accountID = 2 }
                );
                context.SaveChanges();
                //bảng danh mục hoa
                context.Categories.AddRange(
                    new CategoryModel { Name = "Hoa Tươi", Description = "Mô tả cho Hoa Tươi", ImageUrl = "/uploads/type1.png" },
                    new CategoryModel { Name = "Hoa Tình Yêu", Description = "Mô tả cho Hoa Tình Yêu", ImageUrl = "/uploads/type2.png" },
                    new CategoryModel { Name = "Hoa Mai", Description = "Mô tả cho Hoa Mai", ImageUrl = "/uploads/type3.png" },
                    new CategoryModel { Name = "Hoa Mười Giờ", Description = "Mô tả cho Mười Giờ", ImageUrl = "/uploads/type4.png" },
                    new CategoryModel { Name = "Hoa Vạn Thọ", Description = "Mô tả cho Vạn Thọ", ImageUrl = "/uploads/type5.png" },
                    new CategoryModel { Name = "Hoa Kiểng", Description = "Mô tả cho Hoa Kiểng", ImageUrl = "/uploads/type7.png" },
                    new CategoryModel { Name = "Xẻng", Description = "Dụng cụ xẻng dùng để đào và cuốc đất", ImageUrl = "/uploads/type8.png" },
                    new CategoryModel { Name = "Kéo cắt cành", Description = "Dụng cụ kéo dùng để cắt hoặc tỉa cành", ImageUrl = "/uploads/type10.png" },
                    new CategoryModel { Name = "Chậu trồng cây", Description = "Dụng cụ chậu dùng để trồng cây", ImageUrl = "/uploads/type9.png" },
                    new CategoryModel { Name = "Hoa kiểng Độc Lạ Việt Nam", Description = "Mô tả cho Hoa kiểng Độc Lạ Việt Nam", ImageUrl = "/uploads/hoakiengdocla.png" }
                );
                context.SaveChanges();

                //bảng nhà cung cấp
                context.Suppliers.AddRange(
                    new SupplierModel { Name = "Hoa Channel", address = "Chợ Tân Việt Hòa" },
                    new SupplierModel { Name = "Hoa Studio", address = "Chợ Cao Lãnh" },
                    new SupplierModel { Name = "Tân Việt Hòa Market", address = "Chợ Tân Việt Hòa phường 6 thành phố cao lãnh!" },
                    new SupplierModel { Name = "Hội Khuyến Học", address = "Thành Phố Hồ Chí Minh" }
                );
                context.SaveChanges();

                //bảng loại hoa 
                context.Types.AddRange(
                    new TypeModel { Name = "Hoa Tươi", Description = "Mô tả cho hoa Tươi" },
                    new TypeModel { Name = "Hoa Kiểng", Description = "Mô tả cho hoa Kiểng" },
                    new TypeModel { Name = "Hoa Kiểng Độc Lạ", Description = "Mô tả cho hoa Kiểng Độc lạ" },
                    new TypeModel { Name = "Hoa Thân Gỗ", Description = "Mô tả cho hoa" },
                    new TypeModel { Name = "Dụng cụ", Description = "Dụng cụ cho hoa" }
                );
                context.SaveChanges();
                ///bảng thông báo
                context.Notis.AddRange(
                    new Notification
                    {
                        description = "Chào mừng bạn đã đăng ký tài khoản thành công!",
                        Create = DateTime.Now,
                        accountID = 3 // Giả sử người dùng có ID 101
                    },
                    new Notification
                    {
                        description = "Đơn hàng của bạn đã được xác nhận và đang chuẩn bị giao.",
                        Create = DateTime.Now.AddHours(-1),
                        accountID = 2
                    },
                    new Notification
                    {
                        description = "Chào mừng bạn đến với website SHOP HOA - DTHU của chúng tôi!",
                        Create = DateTime.Now.AddDays(-1),
                        accountID = null // Thông báo chung không dành riêng cho tài khoản nào
                    },
                    new Notification
                    {
                        description = "Hệ thống sẽ bảo trì vào lúc 22:00 hằng ngày, mong quý khách thông cảm.",
                        Create = DateTime.Now.AddDays(-1),
                        accountID = null // Thông báo chung không dành riêng cho tài khoản nào
                    },
                    new Notification
                    {
                        description = "Cảm ơn bạn đã đặt hàng, đơn hàng của bạn đang được xử lý.",
                        Create = DateTime.Now.AddMinutes(-30),
                        accountID = 2 // Giả sử người dùng có ID 103
                    }
                );
                context.SaveChanges();

                //bảng bài báo
                context.Newpapers.AddRange(
                    new NewpaperModel { NewpaperTitle = "Gần 100 triệu cành hồng Đà Lạt xuất ra thị trường mỗi năm", Mota = "Ở làng hoa Vạn Thành (Đà Lạt), người dân trồng tới hàng chục loại hoa hồng khác nhau, cung cấp từ 80 đến 100 triệu cành hồng tươi mỗi năm", ImageUrl = "https://i1-vnexpress.vnecdn.net/2017/06/24/Anhhoahong-1498279607.jpg?w=500&h=300&q=100&dpr=1&fit=crop&s=O6c-uW_zO_GwWRCaPrExxw" },
                    new NewpaperModel { NewpaperTitle = "Hồ Ngọc Hà chi gần một tỷ đồng trang trí hoa tươi chụp ảnh", Mota = "Êkíp của ca sĩ nhập hơn 1.500 cành hoa tươi từ nước ngoài để dựng bối cảnh cho bộ ảnh mới.", ImageUrl = "https://i1-giaitri.vnecdn.net/2017/05/08/ho-ngoc-ha-chi-gan-mot-ty-dong-trang-tri-hoa-tuoi-chup-anh-1494217986.jpg?w=500&h=300&q=100&dpr=1&fit=crop&s=uufYHcaacr_X0KEoMx9KLA" },
                    new NewpaperModel { NewpaperTitle = "Vi vu làng hoa Sa Đéc - thủ phủ hoa kiểng nức tiếng Đồng Tháp", Mota = "Được mệnh danh là thủ phủ hoa lớn nhất miền Tây, làng hoa Sa Đéc từ lâu đã trở thành một địa điểm du lịch Đồng Tháp nổi tiếng được nhiều du khách biết đến. Vậy vẻ đẹp của làng hoa Sa Đéc có gì đặc biệt, tham quan tại đây có gì thú vị? Ngay bây giờ hãy cùng Du Lịch Việt tìm hiểu trong bài viết bên dưới bạn nhé" , ImageUrl = "https://dulichviet.com.vn/images/bandidau/du-lich-lang-hoa-sa-dec-dong-thap.jpg" },
                    new NewpaperModel { NewpaperTitle = "Tìm hiểu về lịch sử của làng hoa Sa Đéc - Đồng Tháp", Mota = "Làng hoa Sa Đéc có lịch sử phát triển hơn 100 năm, được biết đến như cái nôi của nghề trồng hoa kiểng tại khu vực miền Tây Nam Bộ. Xuất phát từ những năm đầu thế kỷ XX, nghề trồng hoa ở Sa Đéc ban đầu chỉ là một hoạt động nông nghiệp nhỏ lẻ, phục vụ nhu cầu địa phương. Tuy nhiên, nhờ vào điều kiện thổ nhưỡng phù sa màu mỡ của vùng Đồng Tháp Mười và tay nghề tinh xảo của người dân, làng hoa Sa Đéc đã dần phát triển và mở rộng, được nhiều du khách biết đến." , ImageUrl = "https://dulichviet.com.vn/images/bandidau/lang-hoa-sa-dec-dong-thap-la-cai-noi-cua-nghe-trong-hoa-kieng(1).jpg" },
                    new NewpaperModel { NewpaperTitle = "Du lịch làng hoa Sa Đéc - Đồng Tháp có gì chơi?", Mota = "Tham quan làng hoa Sa Đéc trong tour du lịch Đồng Tháp sẽ là một trải nghiệm thú vị, là dịp để du khách có thể khám phá văn hóa địa phương đến tận hưởng vẻ đẹp thiên nhiên. Dưới đây là một số hoạt động nổi bật khi tham quan làng hoa Sa Đéc - Đồng Tháp", ImageUrl = "https://dulichviet.com.vn/images/bandidau/ve-dep-cua-lang-hoa-sa-dec-dong-thap(1).jpg" },
                    new NewpaperModel { NewpaperTitle = "Làng hoa Sa Đéc tưng bừng sắc màu", Mota = "Festival Hoa – Kiểng Sa Đéc lần thứ I năm 2023 diễn ra từ 30-12-2023 đến 5-1-2024 với rất nhiều hoạt động. Người dân địa phương và du khách có thể đến với Sa Đéc để tận hưởng hương sắc của làng hoa." , ImageUrl = "https://nld.mediacdn.vn/291774122806476800/2023/12/30/1-17039306995912057206732.jpg" }
                );
                context.SaveChanges();
                //hoa kiểng
                string[] kiengNames = {
                    "Cây Hoa Mai", "Cây hoa đào", "Hoa mai đỏ",
                    "Hoa Vạn Thọ", "Vạn Thọ Ban Mai","Cây hoa giấy trang trí dáng đẹp",
                    "Hoa tulip", "Hoa mười giờ kiểng", "Hoa Cúc Mâm Xôi Vàng Trang Trí Tết",
                    "Cúc mâm xôi hồng", "Cây tường vi hoa đỏ huyết long", "Cây Dừa Cạn"
                };

                string[] kiengDescriptions = {
                    "Cây Hoa Mai tượng trưng cho sự giàu sang và thịnh vượng, phổ biến trong ngày Tết.",
                    "Cây hoa đào là biểu tượng của mùa xuân miền Bắc, mang lại may mắn.",
                    "Hoa mai đỏ là sự kết hợp độc đáo giữa sắc đỏ rực rỡ và sự thanh lịch.",
                    "Hoa Vạn Thọ biểu trưng cho sự trường thọ và lòng biết ơn.",
                    "Vạn Thọ Ban Mai có màu sắc tươi sáng, mang lại sức sống mới cho không gian.",
                    "Cây hoa giấy trang trí dáng đẹp, thích hợp để trang trí sân vườn hoặc ban công.",
                    "Hoa tulip, loài hoa kiểng thanh lịch, thường mang ý nghĩa tình yêu hoàn hảo.",
                    "Hoa mười giờ kiểng, dễ trồng, nổi bật với những bông hoa nhỏ rực rỡ.",
                    "Hoa Cúc Mâm Xôi Vàng là sự lựa chọn tuyệt vời để trang trí Tết, tượng trưng cho tài lộc.",
                    "Cúc mâm xôi hồng mang vẻ đẹp dịu dàng, thích hợp cho không gian sống tươi mới.",
                    "Cây tường vi hoa đỏ huyết long, độc đáo với màu hoa đậm và vẻ đẹp mạnh mẽ.",
                    "Cây Dừa Cạn mang ý nghĩa bình yên và dễ chăm sóc, thích hợp làm cây cảnh trong nhà."
                };

                //hoa kiểng độc lạ việt nam
                string[] kiengDocLaNames = {
                    "Kim Điệp - bộ môn hoa lan", "Mai chiếu thủy", "Mai khuyến khích",
                    "Bạc hoa sứ", "Xuân Kỳ - hoa sứ", "Hoa sứ"
                };

                string[] kiengDocLaDescriptions = {
                    "Cây Hoa Lan tượng trưng cho sự giàu sang và thịnh vượng.",
                    "Toàn thân từ gốc lên ngọn của cây mai chiếu thủy được uốn ghép thành hình dạng một chiếc độc bình vô cùng tài tình, khéo léo.",
                    "Hoa mai khuyến khích là sự kết hợp độc đáo giữa sắc đỏ rực rỡ và sự thanh lịch.",
                    "Hoa sứ biểu trưng cho sự trường thọ và lòng biết ơn.",
                    "Xuân Kỳ - hoa sứ có màu sắc tươi sáng, mang lại sức sống mới cho không gian.",
                    "Hoa sứ trang trí dáng đẹp, thích hợp để trang trí sân vườn hoặc ban công."
                };
                //dụng cụ
                string[] toolNames = {
	                "Kéo Cắt Hoa", "Chậu Cây", "Cây Giảm Căng Thẳng",
					"Bình Xịt Nước", "Giỏ Đựng Hoa",
	                "Đũa Cắt Hoa", "Khay Đựng Hoa", "Thùng Đựng Cây"
                };

				string[] toolDescriptions = {
	                "Dụng cụ cắt hoa sắc bén và dễ sử dụng.", "Chậu cây đẹp, chất liệu bền và sang trọng.",
	                "Găng tay làm vườn bảo vệ tay khỏi gai và đất.", "Bình xịt nước giúp tưới cây hiệu quả.", "Cây bút cảnh trang trí đẹp cho không gian.",
	                "Giỏ đựng hoa giúp bảo quản hoa tươi lâu.",
	                "Cây thước cảnh trang trí thêm sinh động cho không gian.", "Bình đựng hoa tươi để trang trí bàn.",
	                "Đũa cắt hoa giúp cắt chính xác và an toàn.", "Khay đựng hoa giúp giữ hoa tươi lâu.",
	                "Thùng đựng cây vườn tiện lợi và dễ sử dụng.", "Bình xịt hương giúp tạo không gian thư giãn."
                };
                //hoa tươi
				string[] names = {
                    "Hoa Tình Yêu Đôi Lứa", "Hoa Hồng Đỏ Rực", "Hoa Mẫu Đơn Kiêu Sa", "Hoa Sen Tinh Khiết",
                    "Hoa Cẩm Chướng Đầy Sắc", "Hoa Tulip Dịu Dàng", "Hoa Bách Hợp Trắng", "Hoa Ly Hồng Phấn",
                    "Hoa Baby Ngọt Ngào", "Hoa Lan Hồ Điệp Tinh Tế", "Hoa Hướng Dương Rạng Rỡ",
                    "Hoa Anh Đào Nhật Bản"
                };

                string[] descriptions = {
                    "Hoa thích hợp cho các buổi hẹn hò lãng mạn.", "Hoa giành tặng cho sinh nhật người thân yêu.",
                    "Hoa đẹp để chúc mừng những dịp đặc biệt.", "Hoa biểu tượng cho tình yêu và sự trong sáng.",
                    "Hoa này phù hợp làm quà tặng dịp lễ.", "Hoa mang đến cảm giác bình yên và thanh tịnh.",
                    "Hoa sang trọng cho sự kiện quan trọng.", "Hoa dành cho các buổi gặp mặt thân mật.",
                    "Hoa tạo niềm vui cho những khoảnh khắc đặc biệt.", "Hoa đem lại sự tươi mới và sức sống.",
                    "Hoa tràn đầy sức sống cho dịp kỷ niệm.", "Hoa gợi lên cảm giác tươi vui và trẻ trung."
                };

                List<ProductModel> products = new List<ProductModel>();
                //dữ liệu hoa tươi
                for (int i = 0; i <= 11; i++)
                {
                    string img = $"/uploads/{i + 1}.jpg";
                    products.Add(new ProductModel
                    {
                        Name = names[i % names.Length],
                        Description = descriptions[i % descriptions.Length],
                        SupplierId = 1,  
                        TypeId = 1,   
                        CategoryId = 2,  // hoa tươi
                        Price = 22000 + (i * 1900), 
                        DisPrice = i % 5 + 1,
                        CreatedByRoleID = 1,
                        Quantity = 100 - i, 
                        ImageUrl = img,
                        typeProduct = 1 // hoa
                    });
                }
				// Dữ liệu dụng cụ
				for (int i = 0; i <= 6; i++)
				{
					string img = $"/uploads/{i + 1}dungcu.png";
					products.Add(new ProductModel
					{
						Name = toolNames[i],
						Description = toolDescriptions[i],
						SupplierId = 2,  
						TypeId = 5,     
						CategoryId = 7,  //dụng cụ
                        Price = 20000 + (i * 2900),  
						DisPrice = i % 5 + 1, 
						CreatedByRoleID = 1,
						Quantity = 50 - i,
						ImageUrl = img,
						typeProduct = 2  //dụng cụ
					});
				}

                // Dữ liệu hoa kiểng
                for (int i = 0; i <= 11; i++)
                {
                    string img = $"/uploads/{i + 1}kieng.png";
                    products.Add(new ProductModel
                    {
                        Name = kiengNames[i],
                        Description = kiengDescriptions[i],
                        SupplierId = 1,
                        TypeId = 2, 
                        CategoryId = 6,//hoa kiểng
                        Price = 21000 + (i * 2500),
                        DisPrice = i % 5 + 1,
                        CreatedByRoleID = 1,
                        Quantity = 50 - i,
                        ImageUrl = img,
                        typeProduct = 1 // hoa
                    });
                }
                // Dữ liệu hoa kiểng độc lạ
                for (int i = 0; i < kiengDocLaNames.Length; i++)
                {
                    string img = $"/uploads/{i + 1}kiengdocla.png";

                    products.Add(new ProductModel
                    {
                        Name = kiengDocLaNames[i],
                        Description = kiengDocLaDescriptions[i],
                        SupplierId = 2,
                        TypeId = 3,     
                        CategoryId = 10,//hoa kiểng độc lạ
                        Price = 2000000 + (i * 290000),
                        DisPrice = i % 5 + 1,
                        CreatedByRoleID = 1,
                        Quantity = 50 - i,
                        ImageUrl = img,
                        typeProduct = 1 // hoa
                    });
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }


        }
    }
}
