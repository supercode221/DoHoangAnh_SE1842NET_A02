using FUNewsManagement_AnalyticsAPI.DAL.Repositories.Interfaces;
using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;
using FUNewsManagement_AnalyticsAPI.DTOs.Tags;
using FUNewsManagement_CoreAPI.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_AnalyticsAPI.DAL.Repositories.Implements
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNMSDbContext _context;

        public NewsArticleRepository(FUNMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<NewsAnalyticsDTO>> GetNewsAnalyticsAsync(
            DateTime? from = null,
            DateTime? to = null,
            int categoryId = 0,
            int tagId = 0)
        {
            var newsQuery = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .AsQueryable();

            if (from.HasValue)
                newsQuery = newsQuery.Where(n => n.CreatedDate >= from.Value);
            if (to.HasValue)
                newsQuery = newsQuery.Where(n => n.CreatedDate <= to.Value);
            if (categoryId > 0)
                newsQuery = newsQuery.Where(n => n.CategoryId == categoryId);
            if (tagId > 0)
                newsQuery = newsQuery.Where(n => n.Tags.Any(t => t.TagId == tagId));

            var newsList = await newsQuery
                .Select(n => new
                {
                    CategoryId = n.CategoryId,
                    CategoryName = n.Category.CategoryName,
                    Status = n.NewsStatus
                })
                .ToListAsync();

            var allCategories = await _context.Categories
                .Select(c => new { c.CategoryId, c.CategoryName })
                .ToListAsync();

            var result = allCategories
                .GroupJoin(
                    newsList,
                    c => c.CategoryId,
                    n => n.CategoryId,
                    (c, newsGroup) => new
                    {
                        c.CategoryName,
                        NewsByStatus = newsGroup
                            .GroupBy(n => n.Status)
                            .Select(g => new NewsAnalyticsDTO
                            {
                                Category = c.CategoryName,
                                Status = g.Key,
                                Count = g.Count()
                            })
                            .DefaultIfEmpty(new NewsAnalyticsDTO
                            {
                                Category = c.CategoryName,
                                Status = false,
                                Count = 0
                            })
                    })
                .SelectMany(c => c.NewsByStatus)
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Status)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetTrendingNewsArticles(
            DateTime? from = null,
            DateTime? to = null,
            int categoryId = 0,
            int tagId = 0)
        {
            var query = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .AsQueryable();

            // Apply filters
            if (from.HasValue)
                query = query.Where(n => n.CreatedDate >= from.Value);

            if (to.HasValue)
                query = query.Where(n => n.CreatedDate <= to.Value);

            if (categoryId > 0)
                query = query.Where(n => n.CategoryId == categoryId);

            if (tagId > 0)
                query = query.Where(n => n.Tags.Any(t => t.TagId == tagId));

            query = query.OrderByDescending(n => n.View);

            var result = await query
                .Select(n => new NewsArticleDTO
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CreatedDate = n.CreatedDate,
                    NewsContent = n.NewsContent,
                    NewsSource = n.NewsSource,
                    CategoryId = n.CategoryId,
                    CategoryName = n.Category.CategoryName,
                    NewsStatus = n.NewsStatus,
                    CreatedById = n.CreatedById,
                    CreatedByName = n.CreatedBy != null ? n.CreatedBy.AccountName : null,
                    UpdatedById = n.UpdatedById,
                    View = n.View,
                    ModifiedDate = n.ModifiedDate,
                    tags = n.Tags.Select(t => new TagDTO
                    {
                        TagId = t.TagId,
                        TagName = t.TagName
                    })
                })
                .Take(10)
                .ToListAsync();

            return result;
        }
    }
}
