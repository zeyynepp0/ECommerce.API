using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            await _service.AddAsync(user);
            return Ok();
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
} 