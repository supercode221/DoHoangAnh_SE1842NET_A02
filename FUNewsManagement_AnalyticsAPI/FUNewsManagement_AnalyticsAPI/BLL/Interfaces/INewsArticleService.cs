using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;

namespace FUNewsManagement_AnalyticsAPI.BLL.Interfaces
{
    public interface INewsArticleService
    {
        Task<List<NewsAnalyticsDTO>> GetNewsAnalyticsAsync(NewsArticlesFilterDTO filter);

        Task<IEnumerable<NewsArticleDTO>> GetTrendingNewsArticles(NewsArticlesFilterDTO filter);
    }
}
