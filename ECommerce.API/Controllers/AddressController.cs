// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Adres gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Adreslerle ilgili API işlemlerini yöneten controller sınıfı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class AddressController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Adres servis katmanı bağımlılığı
        private readonly IAddressService _service; // Adres işlemleri için servis

        // AddressController constructor: Bağımlılığı enjekte eder
        public AddressController(IAddressService service)
        {
            _service = service; // Servisi ata
        }

        // Tüm adresleri getirir
        [HttpGet] // GET isteğiyle çalışır
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm adresleri getir ve döndür

        // Id'ye göre adresi getirir
        [HttpGet("{id}")] // GET isteği, id parametresi ile
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili adresi getir

        // Kullanıcıya ait adresleri getirir
        [HttpGet("user/{userId:int}")] // GET isteği, userId parametresi ile
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var addresses = await _service.GetAddressesByUserIdAsync(userId); // Kullanıcının adreslerini getir
            return Ok(addresses); // Sonucu döndür
        }

        // Yeni adres ekler
        [HttpPost] // POST isteğiyle çalışır
        public async Task<IActionResult> Add([FromBody] AddressUserDto dto)
        {
            var address = new Address
            {
                UserId = dto.UserId, // Kullanıcı ID'si ata
                AddressTitle = dto.AddressTitle, // Adres başlığı ata
                Street = dto.Street, // Sokak bilgisi ata
                City = dto.City, // Şehir ata
                State = dto.State, // Eyalet ata
                PostalCode = dto.PostalCode, // Posta kodu ata
                Country = dto.Country, // Ülke ata
                ContactName = dto.ContactName, // İletişim adı ata
                ContactSurname = dto.ContactSurname, // İletişim soyadı ata
                ContactPhone = dto.ContactPhone // İletişim telefonu ata
            };
            await _service.AddAsync(address); // Adresi ekle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        // Adresi günceller
        [HttpPut] // PUT isteğiyle çalışır
        public async Task<IActionResult> Update([FromBody] AddressUserDto dto)
        {
            var address = new Address
            {
                Id = dto.Id, // Adres ID'si ata
                UserId = dto.UserId, // Kullanıcı ID'si ata
                AddressTitle = dto.AddressTitle, // Adres başlığı ata
                Street = dto.Street, // Sokak bilgisi ata
                City = dto.City, // Şehir ata
                State = dto.State, // Eyalet ata
                PostalCode = dto.PostalCode, // Posta kodu ata
                Country = dto.Country, // Ülke ata
                ContactName = dto.ContactName, // İletişim adı ata
                ContactSurname = dto.ContactSurname, // İletişim soyadı ata
                ContactPhone = dto.ContactPhone // İletişim telefonu ata
            };

            await _service.UpdateAsync(address); // Adresi güncelle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        // Id'ye göre adresi siler
        [HttpDelete("{id}")] // DELETE isteği, id parametresi ile
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id); // Adresi sil
            return Ok(); // Başarılı ise 200 OK döndür
        }


        [HttpGet("test-exception")]
        public IActionResult TestException()
        {
            // Bilerek bir NullReferenceException fırlat
            string test = null;
            var length = test.Length; // Bu satır patlayacak
            return Ok();
        }
    }
} 