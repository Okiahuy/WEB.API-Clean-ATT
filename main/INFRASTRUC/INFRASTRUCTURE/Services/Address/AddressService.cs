using APPLICATIONCORE.Interface.Address;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Address
{
    public class AddressService : IAddressService
    {

        private readonly MyDbContext _context;

        public AddressService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AddressModel>> GetAllAddresses()
        {
            return await _context.Addresss.ToListAsync();
        }
        public async Task<AddressModel> GetAddressById(int addressID)
        {
            return await _context.Addresss.FindAsync(addressID);
        }
        public async Task AddAddress(AddressModel address)
        {
            _context.Addresss.Add(address);
            await _context.SaveChangesAsync();
        }

        private void BadRequest(object value)
        {
            throw new NotImplementedException();
        }

        //Phương thức hiển thị giỏ hàng theo accountID
        public async Task<List<AddressModel>> GetAddressByAccountIDAsync(int accountID)
        {
            var add = await _context.Addresss
                .Where(c => c.accountID == accountID)
                .ToListAsync();

            return add;
        }

        //hàm cập nhật
        public async Task<AddressModel> UpdateAddressAsync(int addressID, AddressModel address)
        {
            var existingAddress = await _context.Addresss.FindAsync(addressID);
            if (existingAddress == null)
            {
                throw new KeyNotFoundException("Không tìm thấy địa chỉ");
            }
            // Cập nhật các thuộc tính khác
            existingAddress.addressName = address.addressName;
            existingAddress.accountID = address.accountID;
            //existingAddress.Create = DateTime.Now;
            existingAddress.city = address.city;
            existingAddress.zipCode = address.zipCode;
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            return existingAddress;
        }

        public async Task DeleteAddress(int addressID)
        {
            var address = await _context.Addresss.FindAsync(addressID);
            if (address == null) throw new KeyNotFoundException("Không tìm thấy địa chỉ để xóa");

            _context.Addresss.Remove(address);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<AddressModel>> SearchAddresses(string addressName)
        {
            return await _context.Addresss
                .Where(p => p.addressName.ToString().Contains(addressName))
                .ToListAsync();
        }
        public async Task<IEnumerable<AddressModel>> FindById(int addressID)
        {
            return await _context.Addresss
                .Where(p => p.addressID.ToString().Contains(addressID.ToString()))
                .ToListAsync();
        }
    }
}
