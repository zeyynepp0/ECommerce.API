using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Review review)
        {
            await _service.AddAsync(review);
            return Ok(review);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Review review)
        {
            await _service.UpdateAsync(review);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("by-product")]
        public async Task<IActionResult> GetReviewsByProduct([FromQuery] int productId)
        {
            var reviews = await _service.GetByProductIdAsync(productId);
            return Ok(reviews);
        }
    }
}