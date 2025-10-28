namespace FUNewsManagement_CoreAPI.BLL.DTOs.Category
{
    public class CategoryViewDTO
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }

        public IEnumerable<CategoryViewDTO>? ChildCategory { get; set; }
    }
}
