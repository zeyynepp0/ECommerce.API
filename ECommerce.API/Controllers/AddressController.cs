using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.DTO;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;
        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var addresses = await _service.GetAddressesByUserIdAsync(userId);
            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddressUserDto dto)
        {
            var address = new Address
            {
                UserId = dto.UserId,
                AddressTitle = dto.AddressTitle,
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                ContactName = dto.ContactName,
                ContactSurname = dto.ContactSurname,
                ContactPhone = dto.ContactPhone
            };
            await _service.AddAsync(address);
            return Ok();
        }

       [HttpPut]
public async Task<IActionResult> Update([FromBody] AddressUserDto dto)
{
    var address = new Address
    {
        Id = dto.Id,
        UserId = dto.UserId,
        AddressTitle = dto.AddressTitle,
        Street = dto.Street,
        City = dto.City,
        State = dto.State,
        PostalCode = dto.PostalCode,
        Country = dto.Country,
        ContactName = dto.ContactName,
        ContactSurname = dto.ContactSurname,
        ContactPhone = dto.ContactPhone
    };

    await _service.UpdateAsync(address);
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