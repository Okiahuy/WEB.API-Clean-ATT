using APPLICATIONCORE.Interface.Account;
using APPLICATIONCORE.Interface.Cart;
using APPLICATIONCORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAccount([FromBody] AccountModel account)
        {
            try
            {
                await _accountService.Register(account);
                return Ok(new { message = "Đăng ký tài khoản thành công", data = account });
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return StatusCode(500, new { message = "Đăng ký tài khoản thất bại", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đăng ký tài khoản thất bại", error = ex.Message });
            }
        }

        [HttpGet("getAccountbyID/{accountID}")]
        public async Task<IActionResult> GetAccountByID(int accountID)
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var acc = await _accountService.GetAccountByAccountIDAsync(accountID);

                if (acc == null || !acc.Any())
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy tài khoản nào",
                    });
                }

                return Ok(new
                {
                    message = "Tìm thấy tài khoản =>",
                    data = acc,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy tài khoản: {ex.Message}", success = false });
            }
        }

        [HttpGet("getAccountByRole")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAccountByRole()
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var acc = await _accountService.GetAccountByRoleIDAsync();

                if (acc == null || !acc.Any())
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy tài khoản nào",
                    });
                }

                return Ok(new
                {
                    message = "Tìm thấy tài khoản =>",
                    data = acc,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy tài khoản: {ex.Message}", success = false });
            }
        }

        [HttpGet("GetNotiByaccountID/{accountID}")]
        public async Task<IActionResult> GetNotiByaccountID(int accountID)
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var acc = await _accountService.GetNotificationByAccountIDAsync(accountID);

                if (acc == null || !acc.Any())
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy tin nhắn nào",
                    });
                }

                return Ok(new
                {
                    message = "Tìm thấy tin nhắn theo người dùng =>",
                    data = acc,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy tin nhắn: {ex.Message}", success = false });
            }
        }
        [HttpGet("GetAllNoti")]
        public async Task<IActionResult> GetAllNoti()
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var acc = await _accountService.GetNotificationsAsync();

                if (acc == null || !acc.Any())
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy tin nhắn nào",
                    });
                }

                return Ok(new
                {
                    message = "Tìm thấy tin nhắn =>",
                    data = acc,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy tin nhắn: {ex.Message}", success = false });
            }
        }

        [HttpGet("GetAllNewpaper")]
        public async Task<IActionResult> GetNewpaperAsync()
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var acc = await _accountService.GetNewpaperAsync();

                if (acc == null || !acc.Any())
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy tin tức nào",
                    });
                }

                return Ok(new
                {
                    message = "Tìm thấy tin tức =>",
                    data = acc,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy tin tức: {ex.Message}", success = false });
            }
        }

    }
}
