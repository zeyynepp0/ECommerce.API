using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    /// <summary>
    /// Kargo firması işlemlerini yöneten controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingCompanyController : ControllerBase
    {
        private readonly IShippingCompanyService _service;
        public ShippingCompanyController(IShippingCompanyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tüm kargo firmalarını getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        /// <summary>
        /// Sadece aktif kargo firmalarını getirir.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActive() => Ok(await _service.GetActiveAsync());

        /// <summary>
        /// Id'ye göre kargo firmasını getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Yeni kargo firması ekler. Sadece Admin yetkisi gerektirir.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] ShippingCompanyDto dto)
        {
            await _service.AddAsync(dto);
            return Ok("Kargo firması eklendi.");
        }

        /// <summary>
        /// Kargo firmasını günceller. Sadece Admin yetkisi gerektirir.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ShippingCompanyDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Kargo firması güncellendi.");
        }

        /// <summary>
        /// Kargo firmasını siler. Kullanımda ise uyarı verir. Sadece Admin yetkisi gerektirir.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return BadRequest("Kargo firması bulunamadı veya kullanımda olduğu için silinemedi.");
            return Ok("Kargo firması silindi.");
        }
    }
} 