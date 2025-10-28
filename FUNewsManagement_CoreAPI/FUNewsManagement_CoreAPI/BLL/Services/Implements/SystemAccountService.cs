using System.Text.RegularExpressions;
using FUNewsManagement_CoreAPI.BLL.DTOs.SystemAccount;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNFUNewsManagement_CoreAPIMS.DAL.Repositories.Interfaces;

namespace FUNewsManagement_CoreAPI.BLL.Services.Implements
{
    public class SystemAccountService : ISystemAccountService
    {

        private readonly ISystemAccountRepository _repository;
        private readonly AuthService _authService;

        public SystemAccountService(ISystemAccountRepository repository, AuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<SystemAccount> CreateAsync(SystemAccountAddDTO account)
        {
            if (await _repository.IsEmailExistsAsync(account.AccountEmail))
                throw new Exception("Email already exists.");

            if (!IsValidPassword(account.AccountPassword))
                throw new Exception("Password must contain at least 8 characters, 1 uppercase, 1 lowercase and 1 speacial!");

            if (string.IsNullOrEmpty(account.AccountName))
                throw new Exception("Name can not be blank or null");

            //if (account.AccountRole != 1 || account.AccountRole != 2)
            //    throw new Exception("Role just can be 1 (Staff) or 2 (Lecturer)");

            short accountId = (short)(_repository.Count() + 1);

            SystemAccount systemAccount = new SystemAccount
            {
                AccountEmail = account.AccountEmail,
                AccountName = account.AccountName,
                AccountId = accountId,
                AccountPassword = _authService.ComputeSha256Hash(account.AccountPassword),
                AccountRole = account.AccountRole
            };

            await _repository.AddAsync(systemAccount);
            await _repository.SaveAsync();
            return systemAccount;
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Account does not exist");

            if (existing.NewsArticles.Any(a => a.CreatedById == existing.AccountId))
            {
                throw new Exception("There is articles associated with this account");
            }

            _repository.Delete(existing);
            await _repository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SystemAccountViewDTO?> GetByIdAsync(short id)
        {
            var account = await _repository.GetByIdAsync(id);

            if (account == null)
                throw new Exception("Account does not exist");

            SystemAccountViewDTO systemAccount = new SystemAccountViewDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
            };

            return systemAccount;
        }

        public async Task<IEnumerable<SystemAccountViewDTO>> SearchAsync(string? keyword, int? accountRole)
        {
            var all = await _repository.GetAllAsync();

            var filtered = all.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filtered = filtered.Where(a =>
                    (!string.IsNullOrEmpty(a.AccountName) &&
                     a.AccountName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    ||
                    (!string.IsNullOrEmpty(a.AccountEmail) &&
                     a.AccountEmail.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                );
            }

            if (accountRole.HasValue)
            {
                filtered = filtered.Where(a => a.AccountRole == accountRole.Value);
            }

            var systemAccounts = filtered.Select(account => new SystemAccountViewDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
            }).ToList();

            return systemAccounts;
        }

        public async Task<SystemAccountViewDTO> UpdateAsync(short id, SystemAccountUpdateDTO account)
        {

            var accountReal = await _repository.GetByIdAsync(id);

            if (string.IsNullOrEmpty(account.AccountName))
                throw new Exception("Name can not be null or blank");

            if (!(account.AccountRole == 1 || account.AccountRole == 2))
                throw new Exception("Role must be 1 (Staff) or 2 (Lecturer)");

            accountReal.AccountName = account.AccountName;
            accountReal.AccountRole = account.AccountRole;

            _repository.Update(accountReal);
            await _repository.SaveAsync();

            SystemAccountViewDTO updated = new SystemAccountViewDTO
            {
                AccountId = accountReal.AccountId,
                AccountName = accountReal.AccountName,
                AccountEmail = accountReal.AccountEmail,
                AccountRole = accountReal.AccountRole,
            };

            return updated;
        }

        public async Task<SystemAccountAuthDTO> GetAuth(string email)
        {
            var account = await _repository.GetByEmailAsync(email);

            SystemAccountAuthDTO auth = new SystemAccountAuthDTO
            {
                AccountEmail = account.AccountEmail,
                AccountPassword = account.AccountPassword,
            };

            return auth;
        }

        public async Task<SystemAccountViewDTO> GetByEmailAsync(string email)
        {
            var account = await _repository.GetByEmailAsync(email);

            SystemAccountViewDTO systemAccount = new SystemAccountViewDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
            };

            return systemAccount;
        }

        public static bool IsValidPassword(string password)
        {
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        public async Task<bool> UpdatePasswordAsync(short id, string curPass, string newPass)
        {
            var accountReal = await _repository.GetByIdAsync(id);

            if (accountReal == null)
                throw new Exception("Account not exist");

            string curPassHash = _authService.ComputeSha256Hash(curPass);

            if (accountReal.AccountPassword != curPassHash)
                throw new Exception("Current password is not valid");

            if (!IsValidPassword(newPass))
            {
                throw new Exception("Password must contain at least 8 characters, 1 uppercase, 1 lowercase and 1 speacial!");
            }

            string newPassHash = _authService.ComputeSha256Hash(newPass);

            accountReal.AccountPassword = newPassHash;

            _repository.Update(accountReal);
            await _repository.SaveAsync();
            return true;
        }
    }
}
