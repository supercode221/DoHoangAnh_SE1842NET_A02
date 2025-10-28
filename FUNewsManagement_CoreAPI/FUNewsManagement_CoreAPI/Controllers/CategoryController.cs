using FUNewsManagement_CoreAPI.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ODataController
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById([FromRoute] short id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditCategoryById([FromRoute] short id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoryById([FromRoute] short id)
        {
            return Ok();
        }
    }
}
