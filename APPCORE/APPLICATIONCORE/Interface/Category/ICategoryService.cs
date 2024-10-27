using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATIONCORE.Models;

namespace APPLICATIONCORE.Interface.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllCategories();
        Task<CategoryModel> GetCategoryById(int id);
        Task AddCategory(CategoryModel category);
        Task<CategoryModel> UpdateCategoryAsync(int id, CategoryModel category);
        Task DeleteCategory(int id);
        Task<IEnumerable<CategoryModel>> SearchCategories(string keyword);
        Task<IEnumerable<CategoryModel>> FindById(int id);
    }
}
