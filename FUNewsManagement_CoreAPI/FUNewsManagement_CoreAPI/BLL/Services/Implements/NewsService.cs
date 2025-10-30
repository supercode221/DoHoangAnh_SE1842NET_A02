using FUNewsManagement_CoreAPI.BLL.DTOs.News;
using FUNewsManagement_CoreAPI.BLL.DTOs.Tags;
using FUNewsManagement_CoreAPI.BLL.Models;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;
using FUNewsManagement_CoreAPI.Middlewares;
using FUNMS.DAL.Repositories.Interfaces;

namespace FUNewsManagement_CoreAPI.BLL.Services.Implements
{
    public class NewsService : INewsService
    {
        private readonly INewsArticleRepository _repo;
        private readonly ITagRepository _tagRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsService(INewsArticleRepository repo, ITagRepository tagRepo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginationResponse<NewsArticleViewDTO>> GetAllAsync(NewsArticleFilter filter)
        {
            var (items, totalCount) = await _repo.GetPagedAsync(filter);

            int totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

            if (filter.Page > totalPages && totalPages > 0)
            {
                filter.Page = totalPages;
                (items, totalCount) = await _repo.GetPagedAsync(filter);
            }

            var mappedItems = items.Select(a => NewsArticleMap(a)).ToList();

            return new PaginationResponse<NewsArticleViewDTO>(mappedItems, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<PaginationResponse<NewsArticleViewDTO>> GetAllPublicAsync(NewsArticleFilter filter)
        {
            filter.NewsStatus = true;

            var (items, totalCount) = await _repo.GetPagedAsync(filter);

            int totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

            if (filter.Page > totalPages && totalPages > 0)
            {
                filter.Page = totalPages;
                (items, totalCount) = await _repo.GetPagedAsync(filter);
            }

            var mappedItems = items.Select(a => NewsArticleMap(a)).ToList();

            return new PaginationResponse<NewsArticleViewDTO>(mappedItems, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<NewsArticleViewDTO> GetNewsDetailAsync(string id)
        {
            var news = await _repo.GetNewsDetailById(id);

            if (news == null)
            {
                throw new Exception($"News Article with id {id} is not exist!");
            }

            var mapped = NewsArticleMap(news);

            return mapped;
        }

        public async Task<NewsArticleViewDTO> GetNewsDetailWithRelatedAsync(string id)
        {
            var (news, related) = await _repo.GetNewsDetailByIdWithRelated(id);

            if (news == null)
                throw new Exception($"News Article with id {id} does not exist!");

            var mappedDetail = NewsArticleMap(news);

            mappedDetail.related = related != null && related.Any()
                ? related.Select(NewsArticleMap).ToList()
                : new List<NewsArticleViewDTO>();

            return mappedDetail;
        }

        public NewsArticleViewDTO NewsArticleMap(NewsArticle a)
        {
            var tags = a.Tags.Select(t => new TagViewDTO()
            {
                TagId = t.TagId,
                TagName = t.TagName,
                Note = t.Note,
            });

            var map = new NewsArticleViewDTO
            {
                NewsArticleId = a.NewsArticleId,
                NewsTitle = a.NewsTitle,
                Headline = a.Headline,
                CreatedDate = a.CreatedDate,
                NewsContent = a.NewsContent,
                NewsSource = a.NewsSource,
                CategoryId = a.CategoryId,
                NewsStatus = a.NewsStatus,
                CreatedById = a.CreatedById,
                UpdatedById = a.UpdatedById,
                ModifiedDate = a.ModifiedDate,
                CategoryName = a.Category?.CategoryName,
                CreatedByName = a.CreatedBy?.AccountName,
                tags = tags,
                View = a.View
            };

            return map;
        }

        public async Task DuplicateNews(string id, short uid)
        {
            var news = await _repo.GetNewsDetailById(id);

            if (news == null)
            {
                throw new Exception($"News article with ID {id} not found!");
            }

            var allNews = await _repo.GetAllAsync();
            var maxId = allNews
                .OrderByDescending(c => int.Parse(c.NewsArticleId))
                .FirstOrDefault()?.NewsArticleId ?? "0";

            int newId = int.Parse(maxId) + 1;

            var duplicatedNews = new NewsArticle
            {
                NewsArticleId = newId.ToString(),
                NewsTitle = news.NewsTitle,
                Headline = news.Headline,
                CreatedDate = DateTime.UtcNow,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryId = news.CategoryId,
                NewsStatus = false,
                CreatedById = uid,
                Tags = news.Tags
            };

            await _repo.AddAsync(duplicatedNews);
            await _repo.SaveAsync();

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "DUPLICATE",
                "NewsArticle",
                news,
                ""
            );
        }

        public async Task AddNewNews(NewsAddDTO dto)
        {
            if (dto == null)
                throw new Exception("Invalid request data.");

            if (string.IsNullOrEmpty(dto.NewsTitle))
                throw new Exception("News title can not be null or blank.");

            if (string.IsNullOrEmpty(dto.Headline))
                throw new Exception("News headline can not be nill or blank.");

            if (string.IsNullOrEmpty(dto.NewsContent))
                throw new Exception("News content can not be null or blank");

            if (dto.CategoryId == null || dto.CategoryId <= 0)
                throw new Exception("News category id can not be null or < 0.");

            if (dto.NewsStatus == null)
                throw new Exception("News Status can just be true or false.");

            if (dto.CreatedById < 1)
                throw new Exception("Invalid created by.");

            var allNews = await _repo.GetAllAsync();
            var maxId = allNews
                .OrderByDescending(c => int.Parse(c.NewsArticleId))
                .FirstOrDefault()?.NewsArticleId ?? "0";

            int newId = int.Parse(maxId) + 1;

            var news = new NewsArticle()
            {
                NewsArticleId = newId.ToString(),
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                NewsContent = dto.NewsContent,
                CreatedDate = DateTime.Now,
                NewsSource = string.IsNullOrEmpty(dto.NewsSource) ? "N/A" : dto.NewsSource,
                CategoryId = dto.CategoryId,
                NewsStatus = dto.NewsStatus,
                CreatedById = dto.CreatedById,
                View = 0
            };

            if (dto.TagIdList != null && dto.TagIdList.Any())
            {
                var tags = await _tagRepo.GetTagsByIdsAsync(dto.TagIdList);

                foreach (var tag in tags)
                {
                    news.Tags.Add(tag);
                }
            }

            await _repo.AddAsync(news);
            await _repo.SaveAsync();

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "ADD",
                "NewsArticle",
                news,
                ""
            );
        }

        public async Task DeleteNewsAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Invalid news ID.");

            var news = await _repo.GetByIdWithTagsAsync(id);

            if (news == null)
                throw new Exception($"News with ID {id} does not exist.");

            if (news.Tags != null && news.Tags.Any())
            {
                news.Tags.Clear();
            }

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "DELETE",
                "NewsArticle",
                news,
                ""
            );

            _repo.Delete(news);
            await _repo.SaveAsync();
        }

        public async Task UpdateNewsAsync(NewsEditDTO dto)
        {
            if (dto == null)
                throw new Exception("Invalid request data.");

            if (string.IsNullOrEmpty(dto.NewsArticleId))
                throw new Exception("News ID cannot be null or blank.");

            if (string.IsNullOrEmpty(dto.NewsTitle))
                throw new Exception("News title cannot be null or blank.");

            if (string.IsNullOrEmpty(dto.Headline))
                throw new Exception("Headline cannot be null or blank.");

            if (string.IsNullOrEmpty(dto.NewsContent))
                throw new Exception("Content cannot be null or blank.");

            if (dto.UpdatedById < 1)
                throw new Exception("Invalid updated by info.");

            var existingNews = await _repo.GetByIdWithTagsAsync(dto.NewsArticleId);
            if (existingNews == null)
                throw new Exception($"News with ID {dto.NewsArticleId} does not exist.");

            var before = new
            {
                existingNews.NewsArticleId,
                existingNews.NewsTitle,
                existingNews.Headline,
                existingNews.NewsContent,
                existingNews.NewsSource,
                existingNews.NewsStatus,
                existingNews.CategoryId,
                Tags = existingNews.Tags?.Select(t => new { t.TagId, t.TagName })
            };

            existingNews.NewsTitle = dto.NewsTitle;
            existingNews.Headline = dto.Headline;
            existingNews.NewsContent = dto.NewsContent;
            existingNews.NewsSource = string.IsNullOrEmpty(dto.NewsSource) ? "N/A" : dto.NewsSource;
            existingNews.NewsStatus = dto.NewsStatus;
            existingNews.CategoryId = dto.CategoryId;
            existingNews.UpdatedById = dto.UpdatedById;
            existingNews.ModifiedDate = DateTime.Now;

            if (dto.TagIdList != null)
            {
                var newTags = await _tagRepo.GetTagsByIdsAsync(dto.TagIdList);
                existingNews.Tags.Clear();
                foreach (var tag in newTags)
                {
                    existingNews.Tags.Add(tag);
                }
            }

            _repo.Update(existingNews);
            await _repo.SaveAsync();

            var after = new
            {
                existingNews.NewsArticleId,
                existingNews.NewsTitle,
                existingNews.Headline,
                existingNews.NewsContent,
                existingNews.NewsSource,
                existingNews.NewsStatus,
                existingNews.CategoryId,
                Tags = existingNews.Tags?.Select(t => new { t.TagId, t.TagName })
            };

            await AuditLogger.LogAsync(
                _httpContextAccessor.HttpContext!,
                "UPDATE",
                "NewsArticle",
                before,
                after
            );
        }

        public async Task IncreaseNewsView(string id)
        {
            await _repo.IncreaseNewsView(id);
        }
    }
}
