using FUNewsManagement_CoreAPI.BLL.DTOs.Tags;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;
using FUNewsManagement_CoreAPI.Middlewares;

namespace FUNewsManagement_CoreAPI.BLL.Services.Implements
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TagService(ITagRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TagViewDTO>> GetAllAsync(string? searchKey)
        {
            var tags = string.IsNullOrWhiteSpace(searchKey)
                ? await _repo.GetAllAsync()
                : await _repo.SearchByNameAsync(searchKey);

            return tags.Select(t => new TagViewDTO
            {
                TagId = t.TagId,
                TagName = t.TagName!,
                Note = t.Note
            });
        }

        public async Task<TagViewDTO?> GetByIdAsync(int id)
        {
            var tag = await _repo.GetTagWithArticlesAsync(id);
            if (tag == null) return null;

            return new TagViewDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName!,
                Note = tag.Note,
                Articles = tag.NewsArticles.Select(a => new ArticleSummaryDTO
                {
                    NewsArticleId = a.NewsArticleId,
                    NewsTitle = a.NewsTitle,
                    Headline = a.Headline
                })
            };
        }

        public async Task AddAsync(TagAddDTO dto)
        {
            var all = await _repo.GetAllAsync();

            if (all.Any(t => t.TagName == dto.TagName))
            {
                throw new Exception("Tag name already existed.");
            }

            int maxId = all
                .OrderByDescending(m => m.TagId)
                .FirstOrDefault().TagId;

            var newTag = new Tag
            {
                TagId = maxId + 1,
                TagName = dto.TagName,
                Note = dto.Note
            };
            await _repo.AddAsync(newTag);
            await _repo.SaveAsync();

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "ADD",
                "Tag",
                newTag,
                ""
            );
        }

        public async Task UpdateAsync(TagEditDTO dto)
        {
            var tag = await _repo.GetByIdAsync(dto.TagId);

            if (tag == null) throw new Exception("Tag not found.");

            if (tag.TagName == dto.TagName) return;

            var tags = await _repo.GetAllAsync();

            if (tags.Any(t => t.TagName == dto.TagName)) throw new Exception("Tag name existed.");

            var oldTag = tag;

            tag.TagName = dto.TagName;
            tag.Note = dto.Note;

            _repo.Update(tag);
            await _repo.SaveAsync();

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "UPDATE",
                "Tag",
                oldTag,
                tag
            );
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _repo.GetTagWithArticlesAsync(id);
            if (tag == null)
                throw new Exception("Tag not found.");

            if (tag.NewsArticles.Any())
                throw new Exception("Cannot delete tag that is referenced by articles.");

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "DELETE",
                "Tag",
                tag,
                ""
            );

            _repo.Delete(tag);
            await _repo.SaveAsync();
        }
    }
}
