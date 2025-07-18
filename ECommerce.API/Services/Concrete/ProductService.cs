// Ürün servisinin iş mantığı ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Ürün varlık sınıfı
using ECommerce.API.Repository.Abstract; // Ürün repository arayüzü
using ECommerce.API.Services.Abstract; // Ürün servis arayüzü
using ECommerce.API.DTO; // DTO sınıfları

namespace ECommerce.API.Services.Concrete
{
    // Ürünlerle ilgili iş mantığını yöneten servis sınıfı
    public class ProductService : IProductService
    {
        // Ürün repository'si (veri erişim katmanı)
        private readonly IProductRepository _repo;

        // ProductService constructor: Repository bağımlılığını enjekte eder
        public ProductService(IProductRepository repo)
        {
            _repo = repo; // Repository'yi ata
        }

        // Tüm ürünleri getirir (ProductDto ile)
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                double rating = 0;
                int reviewCount = 0;
                if (product.Reviews != null && product.Reviews.Count > 0)
                {
                    rating = product.Reviews.Average(r => r.Rating);
                    reviewCount = product.Reviews.Count;
                }
                productDtos.Add(new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    CategoryId = product.CategoryId,
                    ImageUrl = product.ImageUrl,
                    Rating = rating,
                    ReviewCount = reviewCount,
                    CategoryName = product.Category != null ? product.Category.Name : "Kategori Yok"
                });
            }
            return productDtos;
        }

        // Id'ye göre ürünü getirir (ProductDto ile)
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");
            double rating = 0;
            int reviewCount = 0;
            if (product.Reviews != null && product.Reviews.Count > 0)
            {
                rating = product.Reviews.Average(r => r.Rating);
                reviewCount = product.Reviews.Count;
            }
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                Rating = rating,
                ReviewCount = reviewCount,
                CategoryName = product.Category != null ? product.Category.Name : "Kategori Yok"
            };
        }

        // Anahtar kelimeye göre filtrelenmiş ürünleri getirir
        public async Task<List<Product>> GetFilteredAsync(string keyword) => await _repo.GetFilteredAsync(keyword); // Filtreli ürünleri getir

        // Yeni ürün ekler, aynı isimde ürün varsa hata fırlatır
        public async Task AddAsync(Product product)
        {
            var exists = await _repo.FindAsync(p => p.Name == product.Name); // Aynı isimde ürün var mı kontrolü
            if (exists.Any()) throw new Exception("Product already exists"); // Varsa hata

            await _repo.AddAsync(product); // Ürünü ekle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Ürünü günceller
        public async Task UpdateAsync(Product product)
        {
            _repo.Update(product); // Ürünü güncelle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Id'ye göre ürünü siler
        public async Task DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id); // Ürünü getir
            if (product != null)
            {
                _repo.Delete(product); // Ürünü sil
                await _repo.SaveAsync(); // Değişiklikleri kaydet
            }
        }

        // Kategoriye göre ve bir ürünü hariç tutarak ürünleri getirir
        public async Task<List<Product>> GetByCategoryIdAsync(int categoryId, int excludeProductId)
        {
            return await _repo.GetByCategoryIdAsync(categoryId, excludeProductId); // Kategoriye göre ürünleri getir
        }

        // ProductDto ile yeni ürün ekler. Aynı isimde ürün varsa hata fırlatır
        public async Task AddProductAsync(ProductDto dto)
        {
            // Aynı isimde ürün var mı kontrolü
            var exists = await _repo.FindAsync(p => p.Name == dto.Name);
            if (exists.Any()) throw new Exception("Product already exists"); // Varsa hata

            var product = new Product
            {
                Name = dto.Name, // Ürün adı
                Description = dto.Description, // Ürün açıklaması
                Price = dto.Price, // Ürün fiyatı
                Stock = dto.Stock, // Ürün stok miktarı
                CategoryId = dto.CategoryId, // Kategori ID
                ImageUrl = dto.ImageUrl // Ürün görseli
            };
            await _repo.AddAsync(product); // Ürünü ekle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // UpdateProductDto ile var olan ürünü günceller. Ürün yoksa hata fırlatır
        public async Task UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _repo.GetByIdAsync(id); // Ürünü getir
            if (product == null) throw new Exception("Product not found"); // Ürün yoksa hata

            product.Name = dto.Name; // Adı güncelle
            product.Description = dto.Description; // Açıklamayı güncelle
            product.Price = dto.Price; // Fiyatı güncelle
            product.Stock = dto.Stock; // Stok miktarını güncelle
            product.CategoryId = dto.CategoryId; // Kategori ID'yi güncelle
            product.ImageUrl = dto.ImageUrl; // Görseli güncelle

            _repo.Update(product); // Ürünü güncelle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Id ile ürünü siler. Ürün yoksa hata fırlatmaz (sessizce geçer)
        public async Task DeleteProductAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id); // Ürünü getir
            if (product != null)
            {
                _repo.Delete(product); // Ürünü sil
                await _repo.SaveAsync(); // Değişiklikleri kaydet
            }
        }
    }
}

