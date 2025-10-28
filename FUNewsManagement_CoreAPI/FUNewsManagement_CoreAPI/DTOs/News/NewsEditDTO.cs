namespace FUNewsManagement_CoreAPI.BLL.DTOs.News
{
    public class NewsEditDTO
    {
        public string NewsArticleId { get; set; }

        public string? NewsTitle { get; set; }

        public string Headline { get; set; } = null!;

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? UpdatedById { get; set; }

        public IEnumerable<int>? TagIdList { get; set; }
    }
}
