using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECommerce.API.Entities.Abstract;

namespace ECommerce.API.Entities.Concrete
{
    public enum CampaignType
    {
        PercentageDiscount, // Yüzde indirim
        AmountDiscount,     // Tutar bazlı indirim
        BuyXPayY           // 3 al 2 öde gibi
    }

    public class Campaign : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CampaignType Type { get; set; }
        public decimal? Percentage { get; set; } // Yüzde indirim için
        public decimal? Amount { get; set; } // Tutar indirim için
        public int? BuyQuantity { get; set; } // X al
        public int? PayQuantity { get; set; } // Y öde
        public decimal? MinOrderAmount { get; set; } // Tutar indirim için alt limit
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public virtual ICollection<CampaignProduct> CampaignProducts { get; set; } = new List<CampaignProduct>();
        public virtual ICollection<CampaignCategory> CampaignCategories { get; set; } = new List<CampaignCategory>();
    }
} 