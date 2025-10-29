namespace FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle
{
    public class NewsAnalyticsDTO
    {
        public string Category { get; set; } = string.Empty;
        public bool? Status { get; set; }
        public int Count { get; set; }
    }
}
