using APPLICATIONCORE.Interface.AuthLogin;
using APPLICATIONCORE.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            // Tạo token JWT
            var token = GenerateJwtToken(model.Username);
            // Trả về thông tin người dùng cùng với token
            return Ok(new
            {
                FullName = account.FullName,
                roleID = account.roleID,
                Email = account.Email,
                Token = token
            });
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = _configuration.GetValue<string>("Jwt:Key");
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Khóa bảo mật không được cấu hình trong appsettings.json");
            }
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],  // Lấy Issuer từ appsettings.json
                audience: _configuration["Jwt:Audience"],  // Lấy Audience từ appsettings.json
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])), // Thời gian hết hạn từ appsettings.json
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
