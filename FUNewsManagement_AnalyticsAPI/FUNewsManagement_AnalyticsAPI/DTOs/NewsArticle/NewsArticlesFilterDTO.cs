namespace FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle
{
    public class NewsArticlesFilterDTO
    {
        public DateTime? from { get; set; } = null;

        public DateTime? to { get; set; } = null;

        public int categoryId { get; set; } = 0;

        public int tagId { get; set; } = 0;
    }
}
