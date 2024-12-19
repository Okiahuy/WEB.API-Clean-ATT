using APPLICATIONCORE.Interface.Account;
using APPLICATIONCORE.Interface.Cart;
using APPLICATIONCORE.Models;
using APPLICATIONCORE.Models.ViewModel;
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

        [HttpPost("add-answer")]
        public async Task<IActionResult> AddAnswer([FromBody] AddAnswerRquest model)
        {
            try
            {
                await _accountService.AddAnswerAsync(model.accountID, model.productID, model.DescriptionAnswer, model.fullnameAnswer, model.emailAnswer);
                return Ok(new { message = "Bình luận đã được thêm thành công.", data = model });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Đã xảy ra lỗi không mong muốn." });
            }
        }
        [HttpGet("answers/{productId}")]
        public async Task<IActionResult> GetAnswersByProductId(int productId)
        {
            try
            {
                var answers = await _accountService.GetAnswersByProductIdAsync(productId);

                if (answers == null || !answers.Any())
                {
                    return NotFound(new { message = "Không có câu hỏi nào cho sản phẩm này." });
                }

                return Ok(new { message = "Lấy câu hỏi thành công.", data = answers });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Đã xảy ra lỗi không mong muốn." });
            }
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
