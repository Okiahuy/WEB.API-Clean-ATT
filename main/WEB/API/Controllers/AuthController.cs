using APPLICATIONCORE.Interface.AuthLogin;
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


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            // Sử dụng service để xác thực người dùng
            var account = _authService.Authenticate(model.Username, model.Password);

            if (account == null)
            {
                Log.Logger.Information("{@account}");
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            // Tạo token JWT
            var token = GenerateJwtToken(model.Username, Convert.ToInt32(account.roleID));
			// Trả về thông tin người dùng cùng với token
			Log.Logger.Information("{@account}");
			return Ok(new
            {
                FullName = account.FullName,
                roleID = account.roleID,
                Email = account.Email,
                Token = token
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


		//Xử lý phản hồi từ Google
		[HttpGet]
		[Route("google-response")]
		public async Task<IActionResult> GoogleResponse()
		{
			var authenticateResult = await HttpContext.AuthenticateAsync();  // Lấy thông tin xác thực người dùng

			if (!authenticateResult.Succeeded)
			{
				return Unauthorized("Không thể đăng nhập bằng google ngay lúc này!.");
			}
			// Lấy thông tin người dùng từ Google
			var email = authenticateResult.Principal?.FindFirst("email")?.Value;  // Lấy email người dùng
			var name = authenticateResult.Principal?.FindFirst("name")?.Value;    // Lấy tên người dùng

			// Xử lý thông tin người dùng, tạo tài khoản
			return Ok(new { email, name });
		}
	}
}
