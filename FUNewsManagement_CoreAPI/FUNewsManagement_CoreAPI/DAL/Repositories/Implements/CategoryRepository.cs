using FUNewsManagement_CoreAPI.DAL.Data;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_CoreAPI.DAL.Repositories.Implements
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly FUNMSDbContext _context;

        public CategoryRepository(FUNMSDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Category?> GetCategoryById(short id)
        {
            return _context.Categories.Include(c => c.NewsArticles).FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public bool isMainCateValid(string name)
        {
            return !_context.Categories.Any(c => c.CategoryName == name);
        }

        public bool isSubcateValid(short? mainId, string name)
        {
            return !_context.Categories.Any(c => c.ParentCategoryId == mainId && c.CategoryName == name);
        }
    }
}
