using APPLICATIONCORE.Interface.Type;
using APPLICATIONCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet] //lấy tất cả loại
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _typeService.GetAllTypes();
            return Ok(new { message = "Lấy danh sách loại thành công => lụm =>", data = types });
        }

        [HttpGet("{id}")] // lấy loại theo id
        public async Task<IActionResult> GetTypeById(int id)
        {
            var type = await _typeService.GetTypeById(id);
            if (type == null)
            {
                return NotFound(new { message = "Không tìm thấy loại" });
            }
            return Ok(new { message = "Lấy loại thành công => lụm =>", data = type });
        }

        [HttpPost] //thêm lọi
        public async Task<IActionResult> AddType([FromBody] TypeModel type)
        {
            try
            {
                await _typeService.AddType(type);
                return Ok(new { message = "Thêm loại thành công => lụm =>", data = type });
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return StatusCode(500, new { message = "Thêm loại thất bại", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Thêm loại thất bại", error = ex.Message });
            }
        }

        [HttpPut("{id}")] // sửa lọi
        public async Task<IActionResult> UpdateType(int id, [FromBody] TypeModel type)
        {
            try
            {
                var updatedType = await _typeService.UpdateTypeAsync(id, type);
                return Ok(new { message = "Cập nhật loại thành công => lụm =>", data = updatedType });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy loại để cập nhật" });
            }
        }

        [HttpDelete("{id}")] //xóa lọi
        public async Task<IActionResult> DeleteType(int id)
        {
            try
            {
                await _typeService.DeleteType(id);
                return Ok(new { message = "Xóa loại thành công => lụm" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy loại để xóa" });
            }
        }

        [HttpGet("search")] //tìm kím lọi
        public async Task<IActionResult> SearchTypes([FromQuery] string name)
        {
            var types = await _typeService.SearchTypes(name);
            if (types == null || types.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy loại nào" });
            }
            return Ok(new { message = "Tìm kiếm loại thành công => lụm =>", data = types });
        }
    }
}
