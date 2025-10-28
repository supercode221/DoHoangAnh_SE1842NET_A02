using FUNewsManagement_CoreAPI.DAL.Entities;

namespace FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<IEnumerable<Tag>> SearchByNameAsync(string keyword);
        Task<Tag?> GetTagWithArticlesAsync(int id);
        Task<Tag?> GetByIdAsync(int id);
        Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds);
    }
}
