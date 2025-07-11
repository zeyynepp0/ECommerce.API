using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using ECommerce.API.DTO;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Services.Concrete;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _service;
        public CartItemController(ICartItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCartItemsByUserId(int userId)
        {
            var items = await _service.GetCartItemsByUserIdAsync(userId);
            return Ok(items);
        }


        [HttpPost]
        public async Task<IActionResult> Add(CartItemDto cartItemDto)
        {
            try
            {
                if (cartItemDto == null)
                {
                    return BadRequest("CartItem cannot be null");
                }

                if (cartItemDto.UserId <= 0)
                {
                    return BadRequest("UserId is required and must be greater than 0");
                }

                if (cartItemDto.ProductId <= 0)
                {
                    return BadRequest("ProductId is required and must be greater than 0");
                }

                if (cartItemDto.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than 0");
                }

                var cartItem = new CartItem
                {
                    UserId = cartItemDto.UserId,
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity
                };

                await _service.AddAsync(cartItem);
                return Ok(new { message = "Ürün sepete başarıyla eklendi" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Sepete ekleme hatası: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CartItem cartItem)
        {
            await _service.UpdateAsync(cartItem);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete("user/{userId:int}")]
        public async Task<IActionResult> ClearUserCart(int userId)
        {
            await _service.ClearUserCartAsync(userId);
            return Ok();
        }
    }
} 