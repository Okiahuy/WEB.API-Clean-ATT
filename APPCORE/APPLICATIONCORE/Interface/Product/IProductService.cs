using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
		Task<List<ProductModel>> GetProductsByCategoryID(int categoryId);
		Task<List<ProductModel>> GetProductsByType(int typeProduct);
		Task AddProduct(ProductModel product);
        Task<ProductModel> UpdateProductAsync(int id, ProductModel product);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductModel>> SearchProducts(string name);
        Task<IEnumerable<ProductModel>> FindById(int id);
    }
}
