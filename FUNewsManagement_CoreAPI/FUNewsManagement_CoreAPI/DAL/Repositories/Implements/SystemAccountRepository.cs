using FUNewsManagement_CoreAPI.DAL.Data;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Implements;
using FUNFUNewsManagement_CoreAPIMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNMS.DAL.Repositories.Implements
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {

        private readonly FUNMSDbContext _context;

        public SystemAccountRepository(FUNMSDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> GetByIdAsync(short id)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(sa => sa.AccountId == id);
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.SystemAccounts
                .AnyAsync(a => a.AccountEmail == email && (excludeId == null || a.AccountId != excludeId));
        }
    }
}
