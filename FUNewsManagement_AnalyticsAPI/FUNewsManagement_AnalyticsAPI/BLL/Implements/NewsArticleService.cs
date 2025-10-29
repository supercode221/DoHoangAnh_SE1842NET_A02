using System.Data;
using FUNewsManagement_AnalyticsAPI.BLL.Interfaces;
using FUNewsManagement_AnalyticsAPI.DAL.Repositories.Interfaces;
using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;
using FUNewsManagement_AnalyticsAPI.Utils.EPPlus;
using FUNewsManagement_AnalyticsAPI.Utils.FileHandler;

namespace FUNewsManagement_AnalyticsAPI.BLL.Implements
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repo;

        public NewsArticleService(INewsArticleRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> ExportAnalytics()
        {
            AnalyticsDTO analyticsData = await _repo.GetNewsAnalyticsAsync();

            var data = new DataTable();
            data.Columns.Add("Category");
            data.Columns.Add("Published");
            data.Columns.Add("NotPublished");

            foreach (var item in analyticsData.NewsAnalytics)
            {
                data.Rows.Add(item.Category, item.Published, item.NotPublished);
            }

            data.Rows.Add("");

            data.Rows.Add("TOTAL", "Published", analyticsData.NewsStatusAnalytics.TotalPublished);
            data.Rows.Add("TOTAL", "Not Published", analyticsData.NewsStatusAnalytics.TotalNotPublished);

            byte[] excelData = ExcelUtils.ExportToExcel(data);

            string filePath = await FileExporter.ExportToPathAsync(
    excelData,
    $"analytics-{DateTime.Now:yyyyMMdd_HHmmss}"
);

            return filePath;
        }


        public Task<AnalyticsDTO> GetNewsAnalyticsAsync(NewsArticlesFilterDTO filter)
        {
            return _repo.GetNewsAnalyticsAsync(filter.from, filter.to, filter.categoryId, filter.tagId);
        }

        public Task<IEnumerable<NewsArticleDTO>> GetTrendingNewsArticles(NewsArticlesFilterDTO filter)
        {
            return _repo.GetTrendingNewsArticles(filter.from, filter.to, filter.categoryId, filter.tagId);
        }

    }
}
