using APPLICATIONCORE.Interface.Favorite;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavorite([FromQuery] int accountID, [FromQuery] int productID)
        {
            try
            {
                await _favoriteService.ToggleFavoriteAsync(accountID, productID);
                return Ok(new { message = "Yêu thích đã được cập nhật", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi cập nhật yêu thích: {ex.Message}", success = false });
            }
        }
        [HttpGet("isFavorite")]
        public async Task<IActionResult> IsFavorite([FromQuery] int accountID, [FromQuery] int productID)
        {
            try
            {
                var favorite = await _favoriteService.GetFavoriteProductAsync(accountID, productID);

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
                        isFavorite=true,
                        success = true
                    });
                }
                else
                {
                    return Ok(new { message = "Sản phẩm chưa được yêu thích", data = (object)null, isFavorite=false, success = true });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Lỗi khi kiểm tra trạng thái yêu thích", success = false });
            }
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritesByUser([FromQuery] int accountID)
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesByUserAsync(accountID);

                var favoriteProducts = favorites.Select(f => new
                {
                    Id = f.Product.Id,
                    Name = f.Product.Name,
                    Description = f.Product.Description,
                    likecount = f.Product.likecount,
                    Price = f.Product.Price,
                    DisPrice = f.Product.DisPrice,
                    Quantity = f.Product.Quantity,
                    ImageUrl = f.Product.ImageUrl
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
