using FUNewsManagement_CoreAPI.DAL.Data;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_CoreAPI.DAL.Repositories.Implements
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly FUNMSDbContext _context;

        public TagRepository(FUNMSDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> SearchByNameAsync(string keyword)
        {
            return await _context.Tags
                .Where(t => t.TagName!.Contains(keyword))
                .ToListAsync();
        }

        public async Task<Tag?> GetTagWithArticlesAsync(int id)
        {
            return await _context.Tags
                .Include(t => t.NewsArticles)
                .FirstOrDefaultAsync(t => t.TagId == id);
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.TagId == id);
        }

        public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds)
        {
            return await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();
        }
    }
}
