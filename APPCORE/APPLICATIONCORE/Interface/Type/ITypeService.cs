using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Type
{
    public interface ITypeService
    {
        Task<IEnumerable<TypeModel>> GetAllTypes();
        Task<TypeModel> GetTypeById(int id);
        Task AddType(TypeModel type);
        Task<TypeModel> UpdateTypeAsync(int id, TypeModel type);
        Task DeleteType(int id);
        Task<IEnumerable<TypeModel>> SearchTypes(string name);

        Task<IEnumerable<TypeModel>> FindById(int id);
        Task<IEnumerable<ProductModel>> FindProductById(int id);
    }
}
