namespace FUNewsManagement_CoreAPI.BLL.DTOs.Tags
{
    public class TagViewDTO
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = null!;
        public string? Note { get; set; }

        // All articles using this tag
        public IEnumerable<ArticleSummaryDTO>? Articles { get; set; }
    }

    public class ArticleSummaryDTO
    {
        public string NewsArticleId { get; set; } = null!;
        public string NewsTitle { get; set; } = null!;
        public string? Headline { get; set; }
    }
}
