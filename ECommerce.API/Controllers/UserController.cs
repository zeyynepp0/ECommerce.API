using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtService _jwtService;
        private readonly IOrderService _orderService;
        private readonly IReviewService _reviewService;
        public UserController(IUserService service, JwtService jwtService, IOrderService orderService, IReviewService reviewService)
        {
            _service = service;
            _jwtService = jwtService;
            _orderService = orderService;
            _reviewService = reviewService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] User user)
        {
            await _service.AddAsync(user);
            return Ok(user.Id); // Kullanıcı eklendikten sonra ID'yi döndür
        }
                //{
                //  "firstName": "Zeynep",
                //  "lastName": "Can",
                //  "email": "zeynep@example.com",
                //  "passwordHash": "123456",
                //  "addresses": [],
                //  "orders": [],
                //  "reviews": []
                //    }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var user = await _service.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Geçersiz e-posta veya şifre.");
            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        // Sipariş oluşturma
        [HttpPost("orders")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder([FromForm] Order order)
        {
            await _orderService.AddAsync(order);
            return Ok();
        }

        // Yorum ekleme
        [HttpPost("reviews")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddReview([FromForm] Review review)
        {
            await _reviewService.AddAsync(review);
            return Ok();
        }

        // Geçmiş siparişler
        [HttpGet("orders/history/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrderHistory(int userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);
            return Ok(orders);
        }

    [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            await _service.UpdateAsync(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
} 