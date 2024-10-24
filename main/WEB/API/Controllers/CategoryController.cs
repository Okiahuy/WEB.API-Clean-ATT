using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Authorize] // Bảo vệ route này bằng JWT
        public IActionResult GetCategories()
        {

            return Ok(new { message = "Đây là danh sách loại, bạn đã được xác thực! ahihihihihihihihhihihihhihihi" });
        }
    }
}
