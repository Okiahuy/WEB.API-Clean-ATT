using APPLICATIONCORE.Interface.Address;
using APPLICATIONCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _addressService.GetAllAddresses();
            return Ok(new { message = "Lấy danh sách địa chỉ thành công", data = addresses });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _addressService.GetAddressById(id);
            if (address == null)
            {
                return NotFound(new { message = "Không tìm thấy địa chỉ" });
            }
            return Ok(new { message = "Lấy địa chỉ thành công", data = address });
        }

        [HttpGet("getAddressbyID/{accountID}")]
        public async Task<IActionResult> GetAddressByID(int accountID)
        {
            try
            {
                // Truyền trực tiếp object request vào service
                var addr = await _addressService.GetAddressByAccountIDAsync(accountID);

                if (addr == null || !addr.Any())
                {
                    return NotFound(new
                    {
                        success = true,
                        message = "Không tìm thấy địa chỉ nào",
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Tìm thấy địa chỉ =>",
                    data = addr
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy địa chỉ: {ex.Message}", success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressModel address)
        {
           
            try
            {
                await _addressService.AddAddress(address);
                return Ok(new { message = "Thêm địa chỉ thành công", data = address });
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return StatusCode(500, new { message = "Thêm địa chỉ thất bại", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Thêm địa chỉ thất bại", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int addressID, [FromBody] AddressModel address)
        {
            try
            {
                var updatedAddress = await _addressService.UpdateAddressAsync(addressID, address);
                return Ok(new { message = "Cập nhật địa chỉ thành công", data = updatedAddress });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy địa chỉ để cập nhật" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                await _addressService.DeleteAddress(id);
                return Ok(new { message = "Xóa địa chỉ thành công" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy địa chỉ để xóa" });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAddresses([FromQuery] string address)
        {
            var addresses = await _addressService.SearchAddresses(address);
            return Ok(new { message = "Tìm kiếm địa chỉ thành công", data = addresses });
        }
    }
}
