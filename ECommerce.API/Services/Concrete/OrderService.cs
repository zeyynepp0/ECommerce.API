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
        private readonly IShippingCompanyRepository _shippingRepo;
        private readonly INotificationService _notificationService;

        // OrderService constructor: Repository ve context bağımlılıklarını enjekte eder
        public OrderService(IOrderRepository repo, MyDbContext context, IShippingCompanyRepository shippingRepo, INotificationService notificationService)
        {
            _repo = repo; // Repository'yi ata
            _context = context; // Context'i ata
            _shippingRepo = shippingRepo;
            _notificationService = notificationService;
        }

        // Tüm siparişleri getirir
        public async Task<List<Order>> GetAllAsync() => await _repo.GetAllAsync(); // Repository'den tüm siparişleri getir

        // Id'ye göre siparişi getirir
        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var o = await _repo.GetByIdAsync(id);
            if (o == null) return null;
            var campaign = o.CampaignId.HasValue ? await _context.Campaigns.FindAsync(o.CampaignId.Value) : null;
            return new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                UserEmail = o.User?.Email ?? string.Empty,
                CreatedAt = o.CreatedAt,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ShippingCost = o.ShippingCost,
                PaymentMethod = o.PaymentMethod,
                DeliveryPersonName = o.DeliveryPersonName,
                DeliveryPersonPhone = o.DeliveryPersonPhone,
                ShippingCompanyId = o.ShippingCompanyId,
                ShippingCompanyName = o.ShippingCompany?.Name ?? string.Empty,
                AdminStatus = o.AdminStatus,
                AddressId = o.AddressId,
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
                    ProductImage = oi.Product != null ? oi.Product.ImageUrl : string.Empty,
                    ProductDescription = oi.Product?.Description ?? string.Empty,
                    ProductCategory = oi.Product?.Category?.Name ?? string.Empty
                }).ToList(),
                CampaignId = o.CampaignId,
                CampaignName = campaign?.Name,
                CampaignDiscount = o.CampaignDiscount
            };
        }

        // Belirli bir kullanıcıya ait siparişleri, detaylarıyla birlikte getirir
        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Include(o => o.ShippingCompany)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Entity -> DTO mapping
            return orders.Select(o => {
                var campaign = o.CampaignId.HasValue ? _context.Campaigns.Find(o.CampaignId.Value) : null;
                return new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    UserEmail = o.User?.Email ?? string.Empty,
                    CreatedAt = o.CreatedAt,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    ShippingCost = o.ShippingCost,
                    PaymentMethod = o.PaymentMethod,
                    DeliveryPersonName = o.DeliveryPersonName,
                    DeliveryPersonPhone = o.DeliveryPersonPhone,
                    ShippingCompanyId = o.ShippingCompanyId,
                    ShippingCompanyName = o.ShippingCompany?.Name ?? string.Empty,
                    AdminStatus = o.AdminStatus,
                    AddressId = o.AddressId,
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
                    }).ToList(),
                    CampaignId = o.CampaignId,
                    CampaignName = campaign?.Name,
                    CampaignDiscount = o.CampaignDiscount
                };
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
                // Kargo firması ve fiyatı alınır
                var shippingCompany = await _shippingRepo.GetByIdAsync(orderDto.ShippingCompanyId ?? 0);
                if (orderDto.ShippingCompanyId == null || shippingCompany == null || !shippingCompany.IsActive)
                    throw new Exception("Seçilen kargo firması bulunamadı veya aktif değil.");

                // Sipariş kalemlerinin toplamı hesaplanır
                decimal itemsTotal = orderDto.OrderItems.Sum(i => i.UnitPrice * i.Quantity);
                decimal shippingCost = itemsTotal > shippingCompany.FreeShippingLimit ? 0 : shippingCompany.Price;

                decimal campaignDiscount = 0;
                string? campaignName = null;
                if (orderDto.CampaignId.HasValue)
                {
                    var campaign = await _context.Campaigns
                        .Include(c => c.CampaignProducts)
                        .Include(c => c.CampaignCategories)
                        .FirstOrDefaultAsync(c => c.Id == orderDto.CampaignId.Value);

                    if (campaign != null)
                    {
                        campaignName = campaign.Name;
                        // Kampanya kapsamındaki ürün id ve kategori id'leri
                        var campaignProductIds = campaign.CampaignProducts.Select(cp => cp.ProductId).ToList();
                        var campaignCategoryIds = campaign.CampaignCategories.Select(cc => cc.CategoryId).ToList();

                        // Sepette kampanya kapsamındaki ürünler
                        var campaignOrderItems = orderDto.OrderItems.Where(i =>
                            campaignProductIds.Contains(i.ProductId) ||
                            (campaignCategoryIds.Count > 0 && _context.Products.Any(p => p.Id == i.ProductId && campaignCategoryIds.Contains(p.CategoryId)))
                        ).ToList();

                        var campaignProductsTotal = campaignOrderItems.Sum(i => i.UnitPrice * i.Quantity);

                        if (campaign.Type == CampaignType.PercentageDiscount)
                        {
                            campaignDiscount = (campaignProductsTotal * (campaign.Percentage ?? 0)) / 100;
                        }
                        else if (campaign.Type == CampaignType.AmountDiscount)
                        {
                            campaignDiscount = campaignProductsTotal > 0 ? (campaign.Amount ?? 0) : 0;
                        }
                        else if (campaign.Type == CampaignType.BuyXPayY)
                        {
                            // X al Y öde: En ucuz ürüne uygula, toplam adede göre
                            var totalQuantity = campaignOrderItems.Sum(i => i.Quantity);
                            var unitPrice = campaignOrderItems.OrderBy(i => i.UnitPrice).FirstOrDefault()?.UnitPrice ?? 0;
                            var buy = campaign.BuyQuantity ?? 0;
                            var pay = campaign.PayQuantity ?? 0;
                            if (buy > 0 && pay > 0 && totalQuantity >= buy)
                            {
                                var setCount = totalQuantity / buy;
                                campaignDiscount = setCount * (buy - pay) * unitPrice;
                            }
                        }
                    }
                }
                decimal totalAmount = Math.Max(0, itemsTotal + shippingCost - campaignDiscount);

                // DEBUG LOG
                Console.WriteLine($"DEBUG: itemsTotal={itemsTotal}, shippingCost={shippingCost}, campaignDiscount={campaignDiscount}, totalAmount={totalAmount}, campaignId={orderDto.CampaignId}");

                // Sipariş oluştur
                var order = new Order
                {
                    UserId = orderDto.UserId, // Kullanıcı ID
                    AddressId = orderDto.AddressId, // Adres ID
                    ShippingCompanyId = orderDto.ShippingCompanyId, // Kargo firması ID
                    ShippingCost = shippingCost, // Kargo ücreti
                    PaymentMethod = orderDto.PaymentMethod, // Ödeme yöntemi
                    TotalAmount = totalAmount, // İndirimli toplam tutar
                    OrderDate = DateTime.Now, // Sipariş tarihi
                    Status = OrderStatus.Pending, // Sipariş durumu
                    DeliveryPersonName = orderDto.DeliveryPersonName, // Teslimat kişisi
                    DeliveryPersonPhone = orderDto.DeliveryPersonPhone, // Teslimat kişisi telefonu
                    CampaignId = orderDto.CampaignId,
                    CampaignName = campaignName,
                    CampaignDiscount = campaignDiscount,
                    OrderNote = orderDto.OrderNote
                };

                await _context.Orders.AddAsync(order); // Siparişi ekle
                await _context.SaveChangesAsync(); // Kaydet

                // Sipariş kalemlerini oluştur ve stok düş
                foreach (var item in orderDto.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                        throw new Exception($"Ürün bulunamadı: {item.ProductId}");
                    if (product.StockQuantity < item.Quantity)
                        throw new Exception($"{product.Name} ürünü için yeterli stok yok.");
                    product.StockQuantity -= item.Quantity;
                    _context.Products.Update(product);

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _context.OrderItems.AddAsync(orderItem);
                }

                await _context.SaveChangesAsync(); // Kaydet
                await transaction.CommitAsync(); // Transaction'ı tamamla

                // Sipariş oluşturulunca kullanıcıya bildirim gönder
                await _notificationService.AddAsync(new DTO.NotificationDto
                {
                    UserId = order.UserId,
                    Message = $"Siparişiniz alındı, onay bekliyor. Sipariş No: #{order.Id}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

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
        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _repo.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                _repo.Update(order);
                await _repo.SaveAsync();
                // Bildirim gönder
                string message = status switch
                {
                    OrderStatus.Approved => $"Your order has been confirmed by the seller. Order No: {order.Id}",
                    OrderStatus.Preparing => $"Your order is being prepared. Order No: {order.Id}",
                    OrderStatus.Shipped => $"Your order has been shipped. Order No: {order.Id}",
                    OrderStatus.Delivered => $"Your order has been delivered. Order No: {order.Id}",
                    OrderStatus.Cancelled => $"Your order has been cancelled. Order No: {order.Id}",
                    OrderStatus.Returned => $"Your return request has been received. Order No: {order.Id}",
                    OrderStatus.Refunded => $"Your refund has been completed. Order No: {order.Id}",
                    _ => $"Your order status has been updated: {status}. Order No: {order.Id}"
                };
                await _notificationService.AddAsync(new DTO.NotificationDto
                {
                    UserId = order.UserId,
                    Message = message,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });
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
