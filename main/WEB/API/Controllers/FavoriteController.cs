using APPLICATIONCORE.Interface.Favorite;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavorite([FromQuery] int userID, [FromQuery] int productID)
        {
            try
            {
                await _favoriteService.ToggleFavoriteAsync(userID, productID);
                return Ok(new { message = "Yêu thích đã được cập nhật", success = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật yêu thích", success = false });
            }
        }
        [HttpGet("isFavorite")]
        public async Task<IActionResult> IsFavorite([FromQuery] int userID, [FromQuery] int productID)
        {
            try
            {
                var favorite = await _favoriteService.GetFavoriteProductAsync(userID, productID);

                if (favorite != null && favorite.Product != null)
                {
                    return Ok(new
                    {
                        message = "Lấy trạng thái yêu thích thành công",
                        data = new
                        {
                            productID = favorite.productID,
                            productName = favorite.Product.Name ?? "N/A",
                            productDescription = favorite.Product.Description ?? "N/A",
                            productCount = favorite.Product.likecount ?? 0,
                            productPrice = favorite.Product.Price,
                            productDisPrice = favorite.Product.DisPrice,
                            productQuantity = favorite.Product.Quantity,

                        },
                        success = true
                    });
                }
                else
                {
                    return Ok(new { message = "Sản phẩm chưa được yêu thích", data = (object)null, success = true });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Lỗi khi kiểm tra trạng thái yêu thích", success = false });
            }
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritesByUser([FromQuery] int userID)
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesByUserAsync(userID);

                var favoriteProducts = favorites.Select(f => new
                {
                    productID = f.Product.Id,
                    productName = f.Product.Name,
                    productDescription = f.Product.Description,
                    productCount = f.Product.likecount,
                    productPrice = f.Product.Price,
                    productDisPrice = f.Product.DisPrice,
                    productQuantity = f.Product.Quantity,

                }).ToList();

                return Ok(new
                {
                    message = "Lấy danh sách sản phẩm yêu thích thành công",
                    data = favoriteProducts,
                    success = true
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách sản phẩm yêu thích", success = false });
            }
        }


    }
}
