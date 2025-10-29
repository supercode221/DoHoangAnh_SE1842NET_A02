using FUNewsManagement_AnalyticsAPI.DTOs.Tags;

namespace FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle
{
    public class NewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;

        public string? NewsTitle { get; set; }

        public string Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public string? CreatedByName { get; set; }

        public short? UpdatedById { get; set; }

        public long? View { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<NewsArticleDTO> related { get; set; }

        public IEnumerable<TagDTO> tags { get; set; }
    }
}
