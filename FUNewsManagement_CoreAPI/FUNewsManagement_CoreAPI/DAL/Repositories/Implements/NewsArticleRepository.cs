using System.Linq.Expressions;
using FUNewsManagement_CoreAPI.BLL.DTOs.News;
using FUNewsManagement_CoreAPI.DAL.Data;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Implements;
using FUNMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNMS.DAL.Repositories.Implements
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        private readonly FUNMSDbContext _context;

        public NewsArticleRepository(FUNMSDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<NewsArticle?> GetNewsDetailById(string id)
        {
            return _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task<(NewsArticle news, IEnumerable<NewsArticle> related)> GetNewsDetailByIdWithRelated(string id)
        {
            var news = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id && n.NewsStatus == true);

            if (news == null)
                return (null!, Enumerable.Empty<NewsArticle>());

            var categoryId = news.CategoryId;
            var tagIds = news.Tags.Select(t => t.TagId).ToList();

            var relatedQuery = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Where(n =>
                    n.NewsStatus == true &&
                    n.NewsArticleId != id &&
                    (
                        (categoryId.HasValue && n.CategoryId == categoryId) ||
                        n.Tags.Any(t => tagIds.Contains(t.TagId))
                    ))
                .Take(3)
                .AsNoTracking();

            var related = await relatedQuery.ToListAsync();

            return (news, related);
        }

        public async Task<(IEnumerable<NewsArticle> Items, int TotalCount)> GetPagedAsync(NewsArticleFilter filter)
        {
            IQueryable<NewsArticle> query = _context.Set<NewsArticle>()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Include(x => x.CreatedBy);

            if (!string.IsNullOrWhiteSpace(filter.SearchKey))
            {
                var keyword = filter.SearchKey.ToLower();
                query = query.Where(a =>
                    (a.NewsTitle != null && a.NewsTitle.ToLower().Contains(keyword)) ||
                    (a.Headline != null && a.Headline.ToLower().Contains(keyword)) ||
                    (a.NewsSource != null && a.NewsSource.ToLower().Contains(keyword)));
            }

            if (filter.CategoryId.HasValue)
                query = query.Where(a => a.CategoryId == filter.CategoryId);

            if (filter.CreatedById.HasValue)
                query = query.Where(a => a.CreatedById == filter.CreatedById);

            if (filter.NewsStatus.HasValue)
                query = query.Where(a => a.NewsStatus == filter.NewsStatus);

            if (filter.CreatedDate.HasValue)
                query = query.Where(a => a.CreatedDate.HasValue &&
                                         a.CreatedDate.Value.Date == filter.CreatedDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(filter.TagName))
                query = query.Where(a => a.Tags.Any(t => t.TagName == filter.TagName));

            int totalCount = await query.CountAsync();

            query = ApplySorting(query, filter.SortBy, filter.SortOrder);

            var items = await query
                .Skip(filter.Skip)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        private IQueryable<NewsArticle> ApplySorting(IQueryable<NewsArticle> query, string? sortBy, string? sortOrder)
        {
            sortBy ??= "CreatedDate";
            sortOrder = (sortOrder ?? "desc").ToLower();

            var parameter = Expression.Parameter(typeof(NewsArticle), "x");
            var property = Expression.PropertyOrField(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = sortOrder == "asc" ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(NewsArticle), property.Type],
                query.Expression,
                Expression.Quote(lambda)
            );

            return query.Provider.CreateQuery<NewsArticle>(resultExpression);
        }

        public async Task<NewsArticle?> GetByIdWithTagsAsync(string id)
        {
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public void Delete(NewsArticle news)
        {
            _context.NewsArticles.Remove(news);
        }
    }
}
