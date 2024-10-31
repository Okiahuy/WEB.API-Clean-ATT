using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Supplier
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierModel>> GetAllSuppliers();
        Task<SupplierModel> GetSupplierById(int id);
        Task AddSupplier(SupplierModel supplier);
        Task<SupplierModel> UpdateSupplierAsync(int id, SupplierModel supplier);
        Task DeleteSupplier(int id);
        Task<IEnumerable<SupplierModel>> SearchSuppliers(string name);
        Task<IEnumerable<SupplierModel>> FindById(int id);

        Task<IEnumerable<ProductModel>> FindProductById(int id);
    }
}
