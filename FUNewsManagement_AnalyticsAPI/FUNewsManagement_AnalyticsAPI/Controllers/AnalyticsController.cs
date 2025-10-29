using FUNewsManagement_AnalyticsAPI.BLL.Interfaces;
using FUNewsManagement_AnalyticsAPI.DTOs.NewsArticle;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_AnalyticsAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
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
    }
}
