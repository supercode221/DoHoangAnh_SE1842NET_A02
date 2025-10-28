using FUNewsManagement_CoreAPI.BLL.DTOs.News;
using FUNewsManagement_CoreAPI.BLL.Models;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Authorize(Roles = "Staff")]
    [Route("api/news")]
    [ApiController]
    public class NewsController : ODataController
    {

        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet("manage")]
        public async Task<IActionResult> GetAll([FromQuery] NewsArticleFilter filter)
        {
            try
            {
                var result = await _service.GetAllAsync(filter);
                return StatusCode(200, new APIResponse<PaginationResponse<NewsArticleViewDTO>>
                {
                    StatusCode = 200,
                    Data = result
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

        [AllowAnonymous]
        [EnableQuery]
        [HttpGet("")]
        public async Task<IActionResult> GetAllPublic([FromQuery] NewsArticleFilter filter)
        {
            try
            {
                var result = await _service.GetAllPublicAsync(filter);
                return StatusCode(200, new APIResponse<PaginationResponse<NewsArticleViewDTO>>
                {
                    StatusCode = 200,
                    Data = result
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

        [AllowAnonymous]
        [HttpGet("detailWithRelated/{id}")]
        public async Task<IActionResult> GetNewsDetailWithRelated([FromRoute] string id)
        {
            try
            {
                var result = await _service.GetNewsDetailWithRelatedAsync(id);
                return StatusCode(200, new APIResponse<NewsArticleViewDTO>
                {
                    StatusCode = 200,
                    Data = result
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

        [AllowAnonymous]
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetNewsDetail([FromRoute] string id)
        {
            try
            {
                var result = await _service.GetNewsDetailAsync(id);
                return StatusCode(200, new APIResponse<NewsArticleViewDTO>
                {
                    StatusCode = 200,
                    Data = result
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

        [HttpPost("duplicate/{id}")]
        public async Task<IActionResult> DuplicateNews([FromRoute] string id)
        {
            try
            {
                await _service.DuplicateNews(id, short.Parse(User.FindFirst("UserId")?.Value));
                return StatusCode(200, new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Duplicate news success!"
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

        [HttpPost("create")]
        public async Task<IActionResult> AddNewNews([FromBody] NewsAddDTO dto)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                dto.CreatedById = short.Parse(userId);
                await _service.AddNewNews(dto);
                return StatusCode(200, new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "Create new news article success."
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(string id)
        {
            try
            {
                await _service.DeleteNewsAsync(id);
                return Ok(new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = $"News with ID {id} has been deleted successfully."
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

        [HttpPut]
        public async Task<IActionResult> UpdateNews([FromBody] NewsEditDTO dto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                if (short.TryParse(userIdClaim, out short parsedId))
                {
                    dto.UpdatedById = parsedId;
                }
                else
                {
                    throw new Exception("User ID not found or invalid in token.");
                }

                await _service.UpdateNewsAsync(dto);
                return Ok(new APIResponse<string>
                {
                    StatusCode = 200,
                    Message = "News updated successfully."
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
