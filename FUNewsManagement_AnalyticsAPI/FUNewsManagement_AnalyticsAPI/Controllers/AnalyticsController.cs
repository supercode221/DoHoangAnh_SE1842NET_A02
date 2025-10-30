using FUNewsManagement_AnalyticsAPI.BLL.Interfaces;
using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_AnalyticsAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly INewsArticleService _service;

        public AnalyticsController(INewsArticleService service)
        {
            _service = service;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAnalyticsData([FromQuery] NewsArticlesFilterDTO filter)
        {
            var res = await _service.GetNewsAnalyticsAsync(filter);
            return Ok(res);
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingArticles([FromQuery] NewsArticlesFilterDTO filter)
        {
            var res = await _service.GetTrendingNewsArticles(filter);
            return Ok(res);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportAnalytics()
        {
            try
            {
                var filePath = await _service.ExportAnalytics();

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = "Export file not found" });
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                var fileName = Path.GetFileName(filePath);

                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch
                {

                }

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error exporting analytics", error = ex.Message });
            }
        }
    }
}
