using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Favorite
{
    public interface IFavoriteService
    {
        Task ToggleFavoriteAsync(int userID, int productID);
        Task<bool> IsFavoriteAsync(int userID, int productID);

        Task<FavouriteModel> GetFavoriteProductAsync(int userID, int productID);

        Task<List<FavouriteModel>> GetFavoritesByUserAsync(int userID);
    }
}
