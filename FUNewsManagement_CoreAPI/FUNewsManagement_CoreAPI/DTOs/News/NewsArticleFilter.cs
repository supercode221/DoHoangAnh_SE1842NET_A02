using FUNewsManagement_CoreAPI.BLL.Models;

namespace FUNewsManagement_CoreAPI.BLL.DTOs.News
{
    public class NewsArticleFilter : PaginationRequest
    {
        public string? SearchKey { get; set; }
        public short? CategoryId { get; set; }
        public string? TagName { get; set; }
        public short? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? NewsStatus { get; set; }

        public string? SortBy { get; set; } = "CreatedDate";
        public string? SortOrder { get; set; } = "desc";
    }
}
