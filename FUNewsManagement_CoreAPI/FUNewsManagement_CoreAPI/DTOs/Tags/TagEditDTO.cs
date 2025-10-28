namespace FUNewsManagement_CoreAPI.BLL.DTOs.Tags
{
    public class TagEditDTO
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = null!;
        public string? Note { get; set; }
    }
}
