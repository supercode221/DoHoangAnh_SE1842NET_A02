using FUNewsManagement_CoreAPI.BLL.DTOs.News;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;

namespace FUNMS.DAL.Repositories.Interfaces
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<(IEnumerable<NewsArticle> Items, int TotalCount)> GetPagedAsync(NewsArticleFilter filter);
        Task<(NewsArticle? news, IEnumerable<NewsArticle> related)> GetNewsDetailByIdWithRelated(string id);
        Task<NewsArticle?> GetNewsDetailById(string id);
        Task<NewsArticle?> GetByIdWithTagsAsync(string id);
    }
}
