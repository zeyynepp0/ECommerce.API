using ECommerce.API.DTO;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId) => Ok(await _service.GetByUserIdAsync(userId));

        [HttpGet("unread-count/{userId}")]
        public async Task<IActionResult> GetUnreadCount(int userId) => Ok(await _service.GetUnreadCountAsync(userId));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] NotificationDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPost("mark-as-read/{id}")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok();
        }

        [HttpPost("mark-all-as-read/{userId}")]
        [Authorize]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            await _service.MarkAllAsReadAsync(userId);
            return Ok();
        }
    }
} 