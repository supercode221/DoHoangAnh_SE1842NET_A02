using FUNewsManagement_AnalyticsAPI.BLL.Interfaces;
using FUNewsManagement_AnalyticsAPI.DAL.Repositories.Interfaces;
using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;

namespace FUNewsManagement_AnalyticsAPI.BLL.Implements
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repo;

        public NewsArticleService(INewsArticleRepository repo)
        {
            _repo = repo;
        }

        public Task<List<NewsAnalyticsDTO>> GetNewsAnalyticsAsync(NewsArticlesFilterDTO filter)
        {
            return _repo.GetNewsAnalyticsAsync(filter.from, filter.to, filter.categoryId, filter.tagId);
        }

        public Task<IEnumerable<NewsArticleDTO>> GetTrendingNewsArticles(NewsArticlesFilterDTO filter)
        {
            return _repo.GetTrendingNewsArticles(filter.from, filter.to, filter.categoryId, filter.tagId);
        }
    }
}
