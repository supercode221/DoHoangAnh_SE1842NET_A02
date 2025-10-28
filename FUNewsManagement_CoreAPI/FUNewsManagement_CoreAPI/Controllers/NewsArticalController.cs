using FUNewsManagement_CoreAPI.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsArticalController : ODataController
    {
        private readonly INewsArticleService _service;

        public NewsArticalController(INewsArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllNewsArticle()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetNewsArticleById([FromRoute] string id)
        {
            return Ok();
        }

        [HttpPost("duplicate/{id}")]
        public IActionResult DuplicateNewsArticle([FromRoute] string id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateNewsArticle()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNewsArticle(string id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditNewsArticle([FromRoute] string id)
        {
            return Ok();
        }
    }
}
