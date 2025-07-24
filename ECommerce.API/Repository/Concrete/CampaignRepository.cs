using System.Collections.Generic;
using System.Linq;
using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository.Concrete
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly MyDbContext _context;
        public CampaignRepository(MyDbContext context)
        {
            _context = context;
        }

        public Campaign GetById(int id)
        {
            return _context.Campaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignCategories)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Campaign> GetAll()
        {
            return _context.Campaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignCategories)
                .ToList();
        }

        public void Add(Campaign campaign)
        {
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();
        }

        public void Update(Campaign campaign)
        {
            _context.Campaigns.Update(campaign);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var campaign = _context.Campaigns.Find(id);
            if (campaign != null)
            {
                _context.Campaigns.Remove(campaign);
                _context.SaveChanges();
            }
        }

        public void AddProducts(int campaignId, List<int> productIds)
        {
            foreach (var productId in productIds)
            {
                if (!_context.CampaignProducts.Any(cp => cp.CampaignId == campaignId && cp.ProductId == productId))
                {
                    _context.CampaignProducts.Add(new CampaignProduct { CampaignId = campaignId, ProductId = productId });
                }
            }
            _context.SaveChanges();
        }

        public void AddCategories(int campaignId, List<int> categoryIds)
        {
            foreach (var categoryId in categoryIds)
            {
                if (!_context.CampaignCategories.Any(cc => cc.CampaignId == campaignId && cc.CategoryId == categoryId))
                {
                    _context.CampaignCategories.Add(new CampaignCategory { CampaignId = campaignId, CategoryId = categoryId });
                }
            }
            _context.SaveChanges();
        }

        public void RemoveProducts(int campaignId, List<int> productIds)
        {
            var cps = _context.CampaignProducts.Where(cp => cp.CampaignId == campaignId && productIds.Contains(cp.ProductId)).ToList();
            _context.CampaignProducts.RemoveRange(cps);
            _context.SaveChanges();
        }

        public void RemoveCategories(int campaignId, List<int> categoryIds)
        {
            var ccs = _context.CampaignCategories.Where(cc => cc.CampaignId == campaignId && categoryIds.Contains(cc.CategoryId)).ToList();
            _context.CampaignCategories.RemoveRange(ccs);
            _context.SaveChanges();
        }

        public List<Product> GetProductsForCampaign(int campaignId)
        {
            return _context.CampaignProducts.Where(cp => cp.CampaignId == campaignId).Select(cp => cp.Product).ToList();
        }

        public List<Category> GetCategoriesForCampaign(int campaignId)
        {
            return _context.CampaignCategories.Where(cc => cc.CampaignId == campaignId).Select(cc => cc.Category).ToList();
        }
    }
} 