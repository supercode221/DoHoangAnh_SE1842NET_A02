using FUNewsManagement_CoreAPI.DAL.Entities;

namespace FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        bool isMainCateValid(string name);
        bool isSubcateValid(short? mainId, string name);
        Task<Category?> GetCategoryById(short id);
    }
}
