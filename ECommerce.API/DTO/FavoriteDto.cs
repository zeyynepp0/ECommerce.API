// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTO
{
    // Favori ürün verilerini taşımak için kullanılan DTO sınıfı
    public class FavoriteDto
    {
        public int Id { get; set; } // Favori kaydının ID'si
        public int ProductId { get; set; } // Ürün ID'si
        public string ProductName { get; set; } // Ürün adı
        public string ProductImageUrl { get; set; } // Ürün görselinin URL'si
        public decimal Price { get; set; } // Ürün fiyatı
    }
}