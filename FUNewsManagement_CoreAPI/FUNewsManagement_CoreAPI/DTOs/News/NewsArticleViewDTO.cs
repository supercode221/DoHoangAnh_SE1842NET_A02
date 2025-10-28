using FUNewsManagement_CoreAPI.BLL.DTOs.Tags;

namespace FUNewsManagement_CoreAPI.BLL.DTOs.News
{
    public class NewsArticleViewDTO
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

        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<NewsArticleViewDTO> related { get; set; }

        public IEnumerable<TagViewDTO> tags { get; set; }
    }
}
