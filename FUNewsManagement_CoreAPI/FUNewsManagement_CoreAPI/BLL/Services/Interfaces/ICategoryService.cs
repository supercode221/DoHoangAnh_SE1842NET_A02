using FUNewsManagement_CoreAPI.BLL.DTOs.Category;

namespace FUNewsManagement_CoreAPI.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryAddDTO categoryAddDTO);

        Task UpdateCategoryAsync(CategoryEditDTO dto);

        Task DeleteCategoryById(short id);

        Task<IEnumerable<CategoryViewDTO>> GetAllCategory(string searchKey);
    }
}
