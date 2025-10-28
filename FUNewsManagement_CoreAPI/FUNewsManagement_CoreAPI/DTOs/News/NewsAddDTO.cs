namespace FUNewsManagement_CoreAPI.BLL.DTOs.News
{
    public class NewsAddDTO
    {
        public string? NewsTitle { get; set; }

        public string Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public IEnumerable<int>? TagIdList { get; set; }
    }
}
