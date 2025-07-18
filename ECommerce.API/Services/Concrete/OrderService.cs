// Sipariş servisinin iş mantığı ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Sipariş varlık sınıfı
using ECommerce.API.Repository.Abstract; // Sipariş repository arayüzü
using ECommerce.API.Services.Abstract; // Sipariş servis arayüzü
using ECommerce.API.DTO; // DTO sınıfları
using ECommerce.API.Data; // Veritabanı context'i
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

namespace ECommerce.API.Services.Concrete
{
    // Siparişlerle ilgili iş mantığını yöneten servis sınıfı
    public class OrderService : IOrderService
    {
        // Sipariş repository'si (veri erişim katmanı)
        private readonly IOrderRepository _repo;
        // Veritabanı context'i (transaction işlemleri için)
        private readonly MyDbContext _context;

        // OrderService constructor: Repository ve context bağımlılıklarını enjekte eder
        public OrderService(IOrderRepository repo, MyDbContext context)
        {
            _repo = repo; // Repository'yi ata
            _context = context; // Context'i ata
        }

        // Tüm siparişleri getirir
        public async Task<List<Order>> GetAllAsync() => await _repo.GetAllAsync(); // Repository'den tüm siparişleri getir

        // Id'ye göre siparişi getirir
        public async Task<Order> GetByIdAsync(int id) => await _repo.GetByIdAsync(id); // Repository'den siparişi getir

        // Belirli bir kullanıcıya ait siparişleri, detaylarıyla birlikte getirir
        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Entity -> DTO mapping
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                AddressId = o.AddressId,
                ShippingCompany = o.ShippingCompany,
                PaymentMethod = o.PaymentMethod,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate,
                Status = o.Status,
                Address = o.Address != null ? new AddressDto
                {
                    Id = o.Address.Id,
                    AddressTitle = o.Address.AddressTitle,
                    Street = o.Address.Street,
                    City = o.Address.City,
                    State = o.Address.State,
                    PostalCode = o.Address.PostalCode,
                    Country = o.Address.Country,
                    ContactName = o.Address.ContactName,
                    ContactSurname = o.Address.ContactSurname,
                    ContactPhone = o.Address.ContactPhone
                } : null,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product != null ? oi.Product.Name : string.Empty,
                    ProductImage = oi.Product != null ? oi.Product.ImageUrl : string.Empty
                }).ToList()
            }).ToList();
        }

        // Yeni sipariş ekler
        public async Task AddAsync(Order order)
        {
            await _repo.AddAsync(order); // Siparişi ekle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Sipariş DTO'su ile yeni sipariş oluşturur (transaction ile)
        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); // Transaction başlat
            try
            {
                // Sipariş oluştur
                var order = new Order
                {
                    UserId = orderDto.UserId, // Kullanıcı ID
                    AddressId = orderDto.AddressId, // Adres ID
                    ShippingCompany = orderDto.ShippingCompany, // Kargo firması
                    PaymentMethod = orderDto.PaymentMethod, // Ödeme yöntemi
                    TotalAmount = orderDto.TotalAmount, // Toplam tutar
                    OrderDate = DateTime.Now, // Sipariş tarihi
                    Status = "Pending" // Sipariş durumu
                };

                await _context.Orders.AddAsync(order); // Siparişi ekle
                await _context.SaveChangesAsync(); // Kaydet

                // Sipariş kalemlerini oluştur
                foreach (var item in orderDto.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id, // Sipariş ID
                        ProductId = item.ProductId, // Ürün ID
                        Quantity = item.Quantity, // Adet
                        UnitPrice = item.UnitPrice // Birim fiyat
                    };
                    await _context.OrderItems.AddAsync(orderItem); // Sipariş kalemini ekle
                }

                await _context.SaveChangesAsync(); // Kaydet
                await transaction.CommitAsync(); // Transaction'ı tamamla

                return order; // Oluşturulan siparişi döndür
            }
            catch
            {
                await transaction.RollbackAsync(); // Hata olursa geri al
                throw; // Hata fırlat
            }
        }

        // Var olan siparişi günceller
        public async Task UpdateAsync(Order order)
        {
            _repo.Update(order); // Siparişi güncelle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Sipariş durumunu günceller (asenkron)
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _repo.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                _repo.Update(order);
                await _repo.SaveAsync();
            }
        }

        // Id'ye göre siparişi siler (varsa)
        public async Task DeleteAsync(int id)
        {
            var order = await _repo.GetByIdAsync(id); // Siparişi getir
            if (order != null)
            {
                _repo.Delete(order); // Siparişi sil
                await _repo.SaveAsync(); // Değişiklikleri kaydet
            }
        }

        // Kullanıcıya ait siparişleri getirir (asenkron)
        public async Task<List<OrderDto>> GetByUserIdAsync(int userId)
        {
            return await GetOrdersByUserIdAsync(userId);
        }
    }
}
