namespace FUNewsManagement_CoreAPI.BLL.Models
{
    public class APIResponse<T> where T : class
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
