using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;

namespace FUNFUNewsManagement_CoreAPIMS.DAL.Repositories.Interfaces
{
    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
        Task<SystemAccount?> GetByIdAsync(short id);
    }
}
