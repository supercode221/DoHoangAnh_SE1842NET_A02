using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;

namespace FUNewsManagement_AnalyticsAPI.DAL.Repositories.Interfaces
{
    public interface INewsArticleRepository
    {
        Task<AnalyticsDTO> GetNewsAnalyticsAsync(
            DateTime? from = null,
            DateTime? to = null,
            int categoryId = 0,
            int tagId = 0);

        Task<IEnumerable<NewsArticleDTO>> GetTrendingNewsArticles(DateTime? from = null, DateTime? to = null, int categoryId = 0, int tagId = 0);
    }
}
