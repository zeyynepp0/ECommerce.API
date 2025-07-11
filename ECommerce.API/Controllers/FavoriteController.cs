using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _service;

        public FavoriteController(IFavoriteService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserFavorites(int userId)
        {
            var favorites = await _service.GetFavoritesByUserIdAsync(userId);
            return Ok(favorites);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] AddFavoriteRequest request)
        {
            var (success, message) = await _service.AddToFavoritesAsync(request.UserId, request.ProductId);
            if (success)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }


        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] RemoveFavoriteRequest request)
        {
            var result = await _service.RemoveFromFavoritesAsync(request.UserId, request.ProductId);
            if (result)
            {
                return Ok(new { message = "Ürün favorilerden kaldırıldı" });
            }
            return BadRequest(new { message = "Ürün favorilerde bulunamadı" });
        }

        [HttpGet("check/{userId:int}/{productId:int}")]
        public async Task<IActionResult> CheckIfFavorited(int userId, int productId)
        {
            var isFavorited = await _service.IsProductFavoritedByUserAsync(userId, productId);
            return Ok(new { isFavorited });
        }
    }

    public class AddFavoriteRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class RemoveFavoriteRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
} 