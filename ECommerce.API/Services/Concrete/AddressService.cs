// Adres servisinin iş mantığı ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Adres varlık sınıfı
using ECommerce.API.Repository.Abstract; // Adres repository arayüzü
using ECommerce.API.Services.Abstract; // Adres servis arayüzü

namespace ECommerce.API.Services.Concrete
{
    // Adreslerle ilgili iş mantığını yöneten servis sınıfı
    public class AddressService : IAddressService
    {
        // Adres repository'si (veri erişim katmanı)
        private readonly IAddressRepository _repo;

        // AddressService constructor: Repository bağımlılığını enjekte eder
        public AddressService(IAddressRepository repo)
        {
            _repo = repo; // Repository'yi ata
        }

        // Tüm adresleri getirir
        public async Task<List<Address>> GetAllAsync() => await _repo.GetAllAsync(); // Repository'den tüm adresleri getir

        // Id'ye göre adresi getirir
        public async Task<Address> GetByIdAsync(int id) => await _repo.GetByIdAsync(id); // Repository'den adresi getir

        // Yeni adres ekler
        public async Task AddAsync(Address address)
        {
            await _repo.AddAsync(address); // Adresi ekle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Var olan adresi günceller
        public async Task UpdateAsync(Address address)
        {
            _repo.Update(address); // Adresi güncelle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Id'ye göre adresi siler (varsa)
        public async Task DeleteAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id); // Adresi getir
            if (item != null)
            {
                _repo.Delete(item); // Adresi sil
                await _repo.SaveAsync(); // Değişiklikleri kaydet
            }
        }

        // Kullanıcının ilk adresini günceller (örnek iş mantığı)
        public async Task UpdateUserAddressAsync(int userId, Address address)
        {
            // Kullanıcının ilk adresini güncelle (örnek)
            var user = address.User; // Kullanıcıyı al
            if (user == null) throw new Exception("User not found"); // Kullanıcı yoksa hata
            var existing = user.Addresses.FirstOrDefault(); // İlk adresi al
            if (existing == null) throw new Exception("Address not found"); // Adres yoksa hata
            existing.Street = address.Street; // Sokak bilgisini güncelle
            existing.City = address.City; // Şehri güncelle
            existing.State = address.State; // Eyaleti güncelle
            existing.PostalCode = address.PostalCode; // Posta kodunu güncelle
            existing.Country = address.Country; // Ülkeyi güncelle
            _repo.Update(existing); // Adresi güncelle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Belirli bir kullanıcıya ait adresleri getirir
        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _repo.GetAddressesByUserIdAsync(userId); // Kullanıcıya ait adresleri getir
        }
    }
}