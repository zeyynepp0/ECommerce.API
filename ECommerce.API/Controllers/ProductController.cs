using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            await _service.AddAsync(product);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            await _service.UpdateAsync(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("by-category")]
        public async Task<IActionResult> GetByCategory([FromQuery] int categoryId ,[FromQuery] int excludeProductId)
        {
            var products = await _service.GetByCategoryIdAsync(categoryId,  excludeProductId);
            return Ok(products);
        }

        
        [HttpGet("related/{id}")]
        public async Task<IActionResult> GetRelatedProducts(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();

            var related = await _service.GetByCategoryIdAsync(product.CategoryId, product.Id);
            return Ok(related);
        }


    }
} 