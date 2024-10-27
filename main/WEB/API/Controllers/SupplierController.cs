using APPLICATIONCORE.Interface.Supplier;
using APPLICATIONCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliers();
            return Ok(new { message = "Lấy nhà cung cấp thành công", data = suppliers });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierById(id);
            if (supplier == null)
            {
                return NotFound(new { message = "Không tìm thấy nhà cung cấp" });
            }
            return Ok(new { message = "Lấy nhà cung cấp thành công", data = supplier });
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierModel supplier)
        {
            
            try
            {
                await _supplierService.AddSupplier(supplier);
                return Ok(new { message = "Thêm nhà cung cấp thành công", data = supplier });
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return StatusCode(500, new { message = "Thêm nhà cung cấp thất bại", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Thêm nhà cung cấp thất bại", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierModel supplier)
        {
            try
            {
                var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplier);
                return Ok(new { message = "Cập nhật nhà cung cấp thành công", data = updatedSupplier });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy nhà cung cấp để cập nhật" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await _supplierService.DeleteSupplier(id);
                return Ok(new { message = "Xóa nhà cung cấp thành công" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy nhà cung cấp để xóa" });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSuppliers([FromQuery] string name)
        {
            var suppliers = await _supplierService.SearchSuppliers(name);
            if (suppliers == null || suppliers.Count() == 0)
            {
                return NotFound(new { message = "Không tìm thấy nhà cung cấp nào" });
            }
            return Ok(new { message = "Tìm kiếm nhà cung cấp thành công", data = suppliers });
        }
    }
}
