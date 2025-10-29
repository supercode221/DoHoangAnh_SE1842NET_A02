namespace FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle
{
    public class NewsAnalyticsDTO
    {
        public string Category { get; set; } = string.Empty;
        public int Published { get; set; }
        public int NotPublished { get; set; }
    }

    public class NewsStatusAnalyticsDTO
    {
        public int TotalPublished { get; set; }
        public int TotalNotPublished { get; set; }
    }

    public class AnalyticsDTO
    {
        public List<NewsAnalyticsDTO> NewsAnalytics { get; set; } = new();
        public NewsStatusAnalyticsDTO NewsStatusAnalytics { get; set; } = new();
    }
}
