namespace FUNewsManagement_CoreAPI.BLL.DTOs.Category
{
    public class CategoryEditDTO
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public short? CategoryParentId { get; set; }

        public bool IsActive { get; set; }
    }
}
