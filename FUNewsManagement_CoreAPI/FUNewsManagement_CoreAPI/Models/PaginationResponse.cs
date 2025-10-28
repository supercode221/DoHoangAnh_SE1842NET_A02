namespace FUNewsManagement_CoreAPI.BLL.Models
{
    public class PaginationResponse<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasNext => Page < TotalPages;
        public bool HasPrevious => Page > 1;

        public PaginationResponse(IEnumerable<T> items, int totalItems, int page, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }

    }
}
