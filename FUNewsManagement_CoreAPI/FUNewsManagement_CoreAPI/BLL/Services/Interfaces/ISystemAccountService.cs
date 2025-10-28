using FUNewsManagement_CoreAPI.BLL.DTOs.SystemAccount;
using FUNewsManagement_CoreAPI.DAL.Entities;

namespace FUNewsManagement_CoreAPI.BLL.Services.Interfaces
{
    public interface ISystemAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllAsync();
        Task<SystemAccountViewDTO?> GetByIdAsync(short id);
        Task<SystemAccount> CreateAsync(SystemAccountAddDTO account);
        Task<SystemAccountViewDTO> UpdateAsync(short id, SystemAccountUpdateDTO account);
        Task<bool> DeleteAsync(short id);
        Task<IEnumerable<SystemAccountViewDTO>> SearchAsync(string? keyword, int? accountRole);
        Task<SystemAccountAuthDTO> GetAuth(string email);
        Task<SystemAccountViewDTO> GetByEmailAsync(string email);

        Task<bool> UpdatePasswordAsync(short id, string curPass, string newPass);
    }
}
