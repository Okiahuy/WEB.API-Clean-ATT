using APPLICATIONCORE.Interface.Supplier;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Supplier
{
    public class SupplierService : ISupplierService
    {

        private readonly MyDbContext _context;

        public SupplierService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SupplierModel>> GetAllSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }
        public async Task<SupplierModel> GetSupplierById(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }
        public async Task AddSupplier(SupplierModel Supplier)
        {
            _context.Suppliers.Add(Supplier);
            await _context.SaveChangesAsync();
        }

        private void BadRequest(object value)
        {
            throw new NotImplementedException();
        }

        //hàm cập nhật sản phẩm
        public async Task<SupplierModel> UpdateSupplierAsync(int id, SupplierModel supplier)
        {
            // Tìm sản phẩm theo ID
            var existingSupplier = await _context.Suppliers.FindAsync(id);
            if (existingSupplier == null)
            {
                throw new KeyNotFoundException("Không tìm thấy nhà cung cấp");
            }
            // Cập nhật các thuộc tính khác
            existingSupplier.Name = supplier.Name;
            existingSupplier.address = supplier.address;
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingSupplier;
        }

        public async Task DeleteSupplier(int id)
        {
            var Supplier = await _context.Suppliers.FindAsync(id);
            if (Supplier == null) throw new KeyNotFoundException("Không tìm thấy nhà cung cấp để xóa");

            _context.Suppliers.Remove(Supplier);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<SupplierModel>> SearchSuppliers(string name)
        {
            return await _context.Suppliers
                .Where(p => p.Name.ToString().Contains(name))
                .ToListAsync();
        }
        public async Task<IEnumerable<SupplierModel>> FindById(int id)
        {
            return await _context.Suppliers
                .Where(p => p.Id.ToString().Contains(id.ToString()))
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductModel>> FindProductById(int id)
        {
            // Direct equality comparison for accurate results
            return await _context.Products
                .Where(p => p.SupplierId == id)
                .ToListAsync();
        }
    }
}
