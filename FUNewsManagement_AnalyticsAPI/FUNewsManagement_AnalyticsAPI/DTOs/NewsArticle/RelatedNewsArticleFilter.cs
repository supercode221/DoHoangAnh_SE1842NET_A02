namespace FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle
{
    public class RelatedNewsArticleFilter
    {
        public int categoryId { get; set; }

        public int tagId { get; set; }

        public int page { get; set; } = 1;

        public int pageSize { get; set; } = 10;
    }
}
