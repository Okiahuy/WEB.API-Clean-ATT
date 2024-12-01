using APPLICATIONCORE.Interface.Favorite;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using APPLICATIONCORE.Models;
namespace INFRASTRUCTURE.Services.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly MyDbContext _context;

        public FavoriteService(MyDbContext context)
        {
            _context = context;
        }

        public async Task ToggleFavoriteAsync(int accountID, int productID)
        {
            var favorite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.accountID == accountID && f.productID == productID);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productID);

            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại");
            }
            if (favorite != null)
            {
                _context.Favourites.Remove(favorite);
                product.likecount -= 1;
            }
            else
            {
                var newFavorite = new FavouriteModel { accountID = accountID, productID = productID, count = 1 };
                product.likecount += 1;
                await _context.Favourites.AddAsync(newFavorite);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFavoriteAsync(int accountID, int productID)
        {
            return await _context.Favourites
                .AnyAsync(f => f.accountID == accountID && f.productID == productID);
        }

        public async Task<FavouriteModel> GetFavoriteProductAsync(int accountID, int productID)
        {
            return await _context.Favourites
                .Include(f => f.Product) // Giả sử bảng Product có mối quan hệ với Favorite
                .FirstOrDefaultAsync(f => f.accountID == accountID && f.productID == productID);
        }

        public async Task<List<FavouriteModel>> GetFavoritesByUserAsync(int accountID)
        {
            return await _context.Favourites
                .Where(f => f.accountID == accountID)
                .Include(f => f.Product) // Giả sử có quan hệ giữa Favorite và Product
                .ToListAsync();
        }

    }
}
