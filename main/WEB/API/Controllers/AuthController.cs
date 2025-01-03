﻿using APPLICATIONCORE.Interface.AuthLogin;
using APPLICATIONCORE.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Serilog;
using APPLICATIONCORE.Models;
using APPLICATIONCORE.History;
using Microsoft.EntityFrameworkCore;
using INFRASTRUCTURE.Repository;
using Microsoft.Identity.Client;
using APPLICATIONCORE.Interface.Cart;
using System.Security.Principal;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICartService _cartService;

        private readonly IConfiguration _configuration;

        private readonly IAuthService _authService;

        private readonly MyDbContext _context;

        public AuthController(IConfiguration configuration, IAuthService authService, MyDbContext context, ICartService cartService)
        {
            _configuration = configuration;
            _authService = authService;
            _context = context;
            _cartService = cartService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            // Sử dụng service để xác thực người dùng
            var account = _authService.Authenticate(model.Username, model.Password);

            if (account == null)
            {
                Log.Logger.Information("{@account}");
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            // Chờ kết quả từ _cartService
            var cart = await _cartService.GetCartByAccountIDAsync(account.accountID);

            // Tính tổng số lượng sản phẩm trong giỏ hàng
            var totalItems = cart.Count;

            // Tạo token JWT
            var token = GenerateJwtToken(model.Username, Convert.ToInt32(account.roleID));

            // Trả về thông tin người dùng cùng với token và tổng số lượng sản phẩm trong giỏ hàng
            Log.Logger.Information("{@account}");
            return Ok(new
            {
                FullName = account.FullName,
                roleID = account.roleID,
                Email = account.Email,
                Token = token,
                accountID = account.accountID,
                totalItems = totalItems
            });
        }


        private string GenerateJwtToken(string username, int roleID)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("roleID", roleID.ToString()) //xác thực quyền truy cập vào endpoint
                 };

            var key = _configuration.GetValue<string>("Jwt:Key");
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Khóa bảo mật không được cấu hình trong appsettings.json");
            }
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

		//Đăng nhập qua Google
		[HttpGet]
		[Route("signin-google")]
		public IActionResult GoogleLogin()
		{
			var redirectUrl = Url.Action("GoogleResponse");  // Tạo URL quay lại (redirect) sau khi xác thực thành công
			var properties = new AuthenticationProperties { RedirectUri = redirectUrl };  // Cấu hình redirectUri
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);  // Thực hiện thử thách OAuth (yêu cầu xác thực người dùng qua Google)
		}

        [HttpPost]
        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse([FromBody] GoogleTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new { message = "Token không hợp lệ" });
            }

            try
            {
                // Xác thực token với Google API
                var client = new HttpClient();
                var googleEndpoint = $"https://oauth2.googleapis.com/tokeninfo?id_token={request.Token}";
                var response = await client.GetAsync(googleEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    return Unauthorized(new { message = "Xác thực Google không thành công." });
                }

                var userInfo = await response.Content.ReadFromJsonAsync<GoogleUserInfo>();

                if (userInfo == null || string.IsNullOrEmpty(userInfo.Email))
                {
                    return BadRequest(new { message = "Không thể đọc thông tin người dùng từ token." });
                }

                var email = userInfo.Email;
                var name = userInfo.Name ?? "IsGoogle";
                var fullname = userInfo.FullName ?? "IsGoogle";

                // Kiểm tra xem người dùng đã tồn tại
                var existingUser = _context.Accounts.FirstOrDefault(a => a.Email == email);

                if (existingUser != null)
                {
                    var token = GenerateJwtToken(existingUser.UserName, 2);

                    // Chờ kết quả từ _cartService
                    var cart = await _cartService.GetCartByAccountIDAsync(existingUser.accountID);

                    // Tính tổng số lượng sản phẩm trong giỏ hàng
                    var totalItems = cart?.Count ?? 0;

                    return Ok(new
                    {
                        accountID = existingUser.accountID,
                        message = "Người dùng đã tồn tại",
                        FullName = existingUser.UserName,
                        existingUser.Email,
                        roleID = 2,
                        Token = token,
                        totalItems = totalItems
                    });
                }
                else
                {
                    // Thêm mới người dùng
                    var acc = new AccountModel
                    {
                        FullName = name,
                        UserName = name,
                        Email = email,
                        roleID = 2,
                        Password = "IsGoogle",
                        Password2 = "IsGoogle",
                        Phone = "New Phone",
                        isActive = 1,
                        level_cus = 0,
                        ImageUrl = "New Avatar",
                    };

                    _context.Accounts.Add(acc);
                    await _context.SaveChangesAsync();

                    var newToken = GenerateJwtToken(name, 2);
                    // Chờ kết quả từ _cartService
                    var cart = await _cartService.GetCartByAccountIDAsync(acc.accountID);
                    // Tính tổng số lượng sản phẩm trong giỏ hàng
                    var totalItems = cart?.Count ?? 0;

                    return Ok(new
                    {
                        accountID = acc.accountID,
                        FullName = name,
                        roleID = 2,
                        Email = email,
                        Token = newToken,
                        totalItems = totalItems
                    });
                }


               
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Lỗi khi xử lý GoogleResponse: {ErrorMessage}", ex.Message);
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xử lý" });
            }
        }






    }
}
