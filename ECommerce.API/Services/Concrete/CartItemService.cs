// Sepet servisinin iş mantığı ve gerekli kütüphaneler
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için
using ECommerce.API.Entities.Concrete; // Sepet varlık sınıfı
using ECommerce.API.Repository.Abstract; // Sepet repository arayüzü
using ECommerce.API.Services.Abstract; // Sepet servis arayüzü

namespace ECommerce.API.Services.Concrete
{
    // Sepet ürünleriyle ilgili iş mantığını yöneten servis sınıfı
    public class CartItemService : ICartItemService
    {
        // Sepet ürünleri repository'si (veri erişim katmanı)
        private readonly ICartItemRepository _cartItemRepository;

        // CartItemService constructor: Repository bağımlılığını enjekte eder
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository; // Repository'yi ata
        }

        // Tüm sepet ürünlerini getirir
        public async Task<List<CartItem>> GetAllAsync()
        {
            return await _cartItemRepository.GetAllWithIncludesAsync(); // Tüm sepet ürünlerini ilişkili verilerle getir
        }

        // Id'ye göre sepet ürününü getirir
        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _cartItemRepository.GetByIdWithIncludesAsync(id); // Id ile sepet ürününü getir
        }

        // Belirli bir kullanıcıya ait sepet ürünlerini getirir
        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _cartItemRepository.GetCartItemsByUserIdAsync(userId); // Kullanıcıya ait sepet ürünlerini getir
        }

        // Sepete ürün ekler. Eğer ürün zaten sepetteyse miktarını artırır
        public async Task AddAsync(CartItem cartItem)
        {
            var existingItem = await _cartItemRepository.GetCartItemByUserAndProductAsync(cartItem.UserId, cartItem.ProductId); // Kullanıcı ve ürüne göre sepet ürünü var mı kontrol et

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity; // Varsa miktarı artır
                _cartItemRepository.Update(existingItem); // Güncelle
            }
            else
            {
                await _cartItemRepository.AddAsync(cartItem); // Yoksa yeni ürün ekle
            }

            await _cartItemRepository.SaveChangesAsync(); // Değişiklikleri kaydet
        }

        // Var olan sepet ürününü günceller
        public async Task UpdateAsync(CartItem cartItem)
        {
            _cartItemRepository.Update(cartItem); // Sepet ürününü güncelle
            await _cartItemRepository.SaveChangesAsync(); // Değişiklikleri kaydet
        }

        // Id'ye göre sepet ürününü siler (varsa)
        public async Task DeleteAsync(int id)
        {
            var item = await _cartItemRepository.GetByIdAsync(id); // Sepet ürününü getir
            if (item != null)
            {
                _cartItemRepository.Delete(item); // Sepet ürününü sil
                await _cartItemRepository.SaveChangesAsync(); // Değişiklikleri kaydet
            }
        }

        // Kullanıcının sepetini tamamen temizler
        public async Task ClearUserCartAsync(int userId)
        {
            var userCartItems = await _cartItemRepository.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync(); // Kullanıcıya ait tüm sepet ürünlerini getir

            await _cartItemRepository.DeleteRangeAsync(userCartItems); // Tümünü sil
        }
    }
}
