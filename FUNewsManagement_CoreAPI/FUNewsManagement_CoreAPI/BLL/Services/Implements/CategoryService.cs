using FUNewsManagement_CoreAPI.BLL.DTOs.Category;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;

namespace FUNewsManagement_CoreAPI.BLL.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task AddCategoryAsync(CategoryAddDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.name))
                    throw new ArgumentException("Category name cannot be null or blank!");

                if (dto == null || string.IsNullOrWhiteSpace(dto.description))
                    throw new ArgumentException("Category description cannot be null or blank!");

                bool isMainCategory = dto.categoryId == null || dto.categoryId <= 0;

                if (isMainCategory)
                {
                    if (!_repo.isMainCateValid(dto.name))
                        throw new Exception("This main category already exists!");
                }
                else
                {
                    if (!_repo.isSubcateValid(dto.categoryId, dto.name))
                        throw new Exception("This subcategory already exists!");
                }

                var category = new Category
                {
                    CategoryId = dto.id,
                    CategoryName = dto.name,
                    CategoryDesciption = dto.description,
                    ParentCategoryId = isMainCategory ? null : dto.categoryId,
                    IsActive = dto.isActive,
                };

                await _repo.AddAsync(category);
                await _repo.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating new category: {ex.Message}", ex);
            }
        }

        public async Task UpdateCategoryAsync(CategoryEditDTO dto)
        {
            if (dto == null || dto.CategoryId <= 0)
                throw new ArgumentException("Category id is required!");

            if (dto == null || string.IsNullOrWhiteSpace(dto.CategoryName))
                throw new ArgumentException("Category name cannot be null or blank!");

            if (dto == null || string.IsNullOrWhiteSpace(dto.CategoryDescription))
                throw new ArgumentException("Category description cannot be null or blank!");

            var category = await _repo.GetCategoryById(dto.CategoryId);

            if (category == null)
                throw new Exception($"Category with ID {dto.CategoryId} does not exist!");

            if (dto == null || dto.CategoryParentId <= 0)
                dto.CategoryParentId = null;

            bool isMainCategory = dto.CategoryParentId == null || dto.CategoryParentId <= 0;

            if (isMainCategory)
            {
                if (category.CategoryName != dto.CategoryName)
                {
                    if (!_repo.isMainCateValid(dto.CategoryName))
                        throw new Exception("A main category with this name already exists!");
                }
            }
            else
            {
                if (category.CategoryName != dto.CategoryName)
                {
                    if (!_repo.isSubcateValid(dto.CategoryParentId, dto.CategoryName))
                        throw new Exception("A subcategory with this name already exists!");
                }
            }

            if (category.ParentCategoryId != dto.CategoryParentId)
            {
                if (category.NewsArticles != null && category.NewsArticles.Any())
                    throw new Exception("This category is associated with an article and cannot change its parent category.");
            }

            // --- Apply updates ---
            category.CategoryName = dto.CategoryName.Trim();
            category.ParentCategoryId = dto.CategoryParentId;
            category.CategoryDesciption = dto.CategoryDescription;
            category.IsActive = dto.IsActive;

            _repo.Update(category);
            await _repo.SaveAsync();
        }

        public async Task DeleteCategoryById(short id)
        {
            var category = await _repo.GetCategoryById(id);

            if (category == null)
                throw new Exception($"Category with ID {id} does not exist!");

            if (category.NewsArticles != null && category.NewsArticles.Any())
                throw new Exception("This category is associated with an article and cannot delete it.");

            _repo.Delete(category);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<CategoryViewDTO>> GetAllCategory(string searchKey)
        {
            var items = await _repo.GetAllAsync();
            var filtered = items.AsQueryable();

            if (!string.IsNullOrEmpty(searchKey))
            {
                filtered = filtered.Where(c =>
                    (!string.IsNullOrEmpty(c.CategoryName) &&
                     c.CategoryName.Contains(searchKey, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(c.CategoryDesciption) &&
                     c.CategoryDesciption.Contains(searchKey, StringComparison.OrdinalIgnoreCase)));
            }

            var allCategories = filtered.ToList();

            var categoryTree = allCategories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => MapToDTO(c, allCategories))
                .ToList();

            return categoryTree;
        }

        private CategoryViewDTO MapToDTO(Category category, List<Category> allCategories)
        {
            return new CategoryViewDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive,
                ChildCategory = allCategories
                    .Where(child => child.ParentCategoryId == category.CategoryId)
                    .Select(child => MapToDTO(child, allCategories))
                    .ToList()
            };
        }

    }
}
