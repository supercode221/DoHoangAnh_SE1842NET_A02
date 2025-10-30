using FUNewsManagement_CoreAPI.BLL.DTOs.Tags;

namespace FUNewsManagement_CoreAPI.BLL.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagViewDTO>> GetAllAsync(string? searchKey);
        Task<TagViewDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(TagAddDTO dto);
        Task UpdateAsync(TagEditDTO dto);
        Task DeleteAsync(int id);
    }
}
