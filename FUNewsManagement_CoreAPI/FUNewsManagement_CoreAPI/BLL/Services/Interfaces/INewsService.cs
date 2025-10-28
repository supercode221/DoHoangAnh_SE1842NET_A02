using FUNewsManagement_CoreAPI.BLL.DTOs.News;
using FUNewsManagement_CoreAPI.BLL.Models;

namespace FUNewsManagement_CoreAPI.BLL.Services.Interfaces
{
    public interface INewsService
    {
        Task<PaginationResponse<NewsArticleViewDTO>> GetAllAsync(NewsArticleFilter filter);
        Task<PaginationResponse<NewsArticleViewDTO>> GetAllPublicAsync(NewsArticleFilter filter);
        Task<NewsArticleViewDTO> GetNewsDetailAsync(string id);
        Task<NewsArticleViewDTO> GetNewsDetailWithRelatedAsync(string id);
        Task DuplicateNews(string id, short uid);
        Task AddNewNews(NewsAddDTO dto);
        Task DeleteNewsAsync(string id);
        Task UpdateNewsAsync(NewsEditDTO dto);
    }
}
