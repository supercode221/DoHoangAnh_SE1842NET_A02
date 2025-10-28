using FUNewsManagement_CoreAPI.BLL.DTOs.Category;
using FUNewsManagement_CoreAPI.BLL.Models;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCategory([FromQuery] string? searchKey)
        {
            try
            {
                var items = await _service.GetAllCategory(searchKey);
                return StatusCode(200, new APIResponse<IEnumerable<CategoryViewDTO>>
                {
                    StatusCode = 200,
                    Data = items
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryAddDTO dto)
        {
            try
            {
                await _service.AddCategoryAsync(dto);
                return StatusCode(201, new APIResponse<string>
                {
                    StatusCode = 201,
                    Message = "Category created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPut]
        public async Task<IActionResult> EditCategory([FromBody] CategoryEditDTO dto)
        {
            try
            {
                await _service.UpdateCategoryAsync(dto);
                return StatusCode(200, new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Category updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] short id)
        {
            try
            {
                await _service.DeleteCategoryById(id);
                return StatusCode(200, new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Category deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }
    }
}
