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
        Task<List<ProductModel>> GetProductsByCategoryIDandQuantity(int categoryId, int sl);
        Task<List<ProductModel>> GetProductsByType(int typeProduct, int sl);

        Task<List<ProductModel>> GetProductsByTypeAsync(int typeProduct, int page, int pageSize);

        Task AddProduct(ProductModel product);
        Task<ProductModel> UpdateProductAsync(int id, ProductModel product);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductModel>> SearchProducts(string name);
        Task<IEnumerable<ProductModel>> FindById(int id);

        Task<List<ProductModel>> GetProductsByCategoryAndPriceRange(int categoryId, decimal minPrice, decimal maxPrice, string sortBy);

        Task<IEnumerable<ProductModel>> GetProductsByCategoryAndPriceRange(int categoryId, decimal minPrice, decimal maxPrice);
    }
}
