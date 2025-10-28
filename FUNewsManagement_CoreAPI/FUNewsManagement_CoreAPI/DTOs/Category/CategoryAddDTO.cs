namespace FUNewsManagement_CoreAPI.BLL.DTOs.Category
{
    public class CategoryAddDTO
    {
        public short id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public short categoryId { get; set; }

        public bool? isActive { get; set; }
    }
}
