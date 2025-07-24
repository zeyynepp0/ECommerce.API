using Microsoft.AspNetCore.Mvc;
using ECommerce.API.DTO;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _service;
        public CampaignController(ICampaignService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var campaign = _service.GetById(id);
            if (campaign == null) return NotFound();
            return Ok(campaign);
        }

        [HttpPost]
        public IActionResult Add([FromBody] CampaignDto dto)
        {
            _service.Add(dto);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] CampaignDto dto)
        {
            _service.Update(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

        [HttpPost("{id}/products")]
        public IActionResult AddProducts(int id, [FromBody] List<int> productIds)
        {
            _service.AddProducts(id, productIds);
            return Ok();
        }

        [HttpPost("{id}/categories")]
        public IActionResult AddCategories(int id, [FromBody] List<int> categoryIds)
        {
            _service.AddCategories(id, categoryIds);
            return Ok();
        }

        [HttpDelete("{id}/products")]
        public IActionResult RemoveProducts(int id, [FromBody] List<int> productIds)
        {
            _service.RemoveProducts(id, productIds);
            return Ok();
        }

        [HttpDelete("{id}/categories")]
        public IActionResult RemoveCategories(int id, [FromBody] List<int> categoryIds)
        {
            _service.RemoveCategories(id, categoryIds);
            return Ok();
        }

        [HttpPost("{id}/toggle-active")]
        public IActionResult ToggleActive(int id)
        {
            _service.ToggleActive(id);
            return Ok();
        }

        [HttpPost("eligible")]
        public IActionResult GetEligibleCampaigns([FromBody] EligibleCampaignRequestDto dto)
        {
            var campaigns = _service.GetAll()
                .Where(c => c.IsActive &&
                    (c.ProductIds.Any(pid => dto.ProductIds.Contains(pid)) ||
                     c.CategoryIds.Any(cid => dto.CategoryIds.Contains(cid))))
                .ToList();
            return Ok(campaigns);
        }
    }
} 