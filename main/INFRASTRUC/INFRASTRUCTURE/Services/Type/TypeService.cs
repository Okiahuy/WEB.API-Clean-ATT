
using APPLICATIONCORE.Interface.Supplier;
using APPLICATIONCORE.Interface.Type;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Type
{
    public class TypeService : ITypeService
    {

        private readonly MyDbContext _context;

        public TypeService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TypeModel>> GetAllTypes()
        {
            return await _context.Types.ToListAsync();
        }
        public async Task<TypeModel> GetTypeById(int id)
        {
            return await _context.Types.FindAsync(id);
        }
        public async Task AddType(TypeModel type)
        {
            _context.Types.Add(type);
            await _context.SaveChangesAsync();
        }

        private void BadRequest(object value)
        {
            throw new NotImplementedException();
        }

        //hàm cập nhật sản phẩm
        public async Task<TypeModel> UpdateTypeAsync(int id, TypeModel type)
        {
            // Tìm sản phẩm theo ID
            var existingtype = await _context.Types.FindAsync(id);
            if (existingtype == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Loại để sửa");
            }
            // Cập nhật các thuộc tính khác
            existingtype.Name = type.Name;
            existingtype.Description = type.Description;
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingtype;
        }

        public async Task DeleteType(int id)
        {
            var type = await _context.Types.FindAsync(id);
            if (type == null) throw new KeyNotFoundException("Không tìm thấy Loại để xóa");

            _context.Types.Remove(type);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<TypeModel>> SearchTypes(string name)
        {
            return await _context.Types
                .Where(p => p.Name.ToString().Contains(name))
                .ToListAsync();
        }
        public async Task<IEnumerable<TypeModel>> FindById(int id)
        {
            return await _context.Types
                .Where(p => p.Id.ToString().Contains(id.ToString()))
                .ToListAsync();
        }
    }
}
