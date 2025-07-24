using System.Collections.Generic;
using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface ICampaignRepository
    {
        Campaign GetById(int id);
        IEnumerable<Campaign> GetAll();
        void Add(Campaign campaign);
        void Update(Campaign campaign);
        void Delete(int id);
        void AddProducts(int campaignId, List<int> productIds);
        void AddCategories(int campaignId, List<int> categoryIds);
        void RemoveProducts(int campaignId, List<int> productIds);
        void RemoveCategories(int campaignId, List<int> categoryIds);
        List<Product> GetProductsForCampaign(int campaignId);
        List<Category> GetCategoriesForCampaign(int campaignId);
    }
} 