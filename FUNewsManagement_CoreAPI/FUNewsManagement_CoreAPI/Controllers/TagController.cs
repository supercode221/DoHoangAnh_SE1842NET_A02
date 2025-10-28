using FUNewsManagement_CoreAPI.BLL.DTOs.Tags;
using FUNewsManagement_CoreAPI.BLL.Models;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchKey)
        {
            try
            {
                var tags = await _service.GetAllAsync(searchKey);
                return StatusCode(200, new APIResponse<IEnumerable<TagViewDTO>>()
                {
                    StatusCode = 200,
                    Data = tags
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var tag = await _service.GetByIdAsync(id);

                if (tag == null) return NotFound("Tag not found.");

                return StatusCode(200, new APIResponse<TagViewDTO>()
                {
                    StatusCode = 200,
                    Data = tag
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
        public async Task<IActionResult> Add([FromBody] TagAddDTO dto)
        {
            try
            {
                await _service.AddAsync(dto);

                return StatusCode(200, new APIResponse<string>()
                {
                    StatusCode = 200,
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
        public async Task<IActionResult> Update([FromBody] TagEditDTO dto)
        {
            try
            {
                await _service.UpdateAsync(dto);

                return StatusCode(200, new APIResponse<string>()
                {
                    StatusCode = 200,
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return StatusCode(200, new APIResponse<string>()
                {
                    StatusCode = 200,
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
