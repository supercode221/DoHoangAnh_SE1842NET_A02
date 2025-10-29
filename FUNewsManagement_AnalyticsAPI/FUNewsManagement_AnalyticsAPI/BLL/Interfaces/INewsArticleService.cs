using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;

namespace FUNewsManagement_AnalyticsAPI.BLL.Interfaces
{
    public interface INewsArticleService
    {
        Task<AnalyticsDTO> GetNewsAnalyticsAsync(NewsArticlesFilterDTO filter);

        Task<IEnumerable<NewsArticleDTO>> GetTrendingNewsArticles(NewsArticlesFilterDTO filter);

        Task<string> ExportAnalytics();
    }
}
