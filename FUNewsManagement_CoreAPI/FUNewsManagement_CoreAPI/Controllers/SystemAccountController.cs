using FUNewsManagement_CoreAPI.BLL.DTOs.SystemAccount;
using FUNewsManagement_CoreAPI.BLL.Models;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class SystemAccountController : ODataController
    {
        private readonly ISystemAccountService _service;

        public SystemAccountController(ISystemAccountService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _service.GetAllAsync();

            APIResponse<IEnumerable<SystemAccount>> response = new APIResponse<IEnumerable<SystemAccount>>
            {
                StatusCode = 200,
                Data = accounts
            };

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            try
            {
                var account = await _service.GetByIdAsync(id);

                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(new APIResponse<string> { StatusCode = 404, Message = "Account does not exist!" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword, [FromQuery] int? accountRole)
        {
            var result = await _service.SearchAsync(keyword, accountRole);

            APIResponse<IEnumerable<SystemAccountViewDTO>> response;

            if (result.IsNullOrEmpty())
            {
                response = new APIResponse<IEnumerable<SystemAccountViewDTO>>
                {
                    StatusCode = 404,
                    Message = "No user found",
                    Data = null
                };

                return NotFound(response);
            }
            else
            {
                response = new APIResponse<IEnumerable<SystemAccountViewDTO>>
                {
                    StatusCode = 200,
                    Message = "Get list user success",
                    Data = result
                };
                return Ok(response);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemAccountAddDTO account)
        {
            try
            {
                var created = await _service.CreateAsync(account);
                return CreatedAtAction(nameof(GetById), new { id = created.AccountId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string> { StatusCode = 400, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, [FromBody] SystemAccountUpdateDTO account)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, account);
                return Ok(new APIResponse<SystemAccountViewDTO> { StatusCode = 200, Message = "Update account success", Data = updated });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string> { StatusCode = 400, Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);

                return Ok(new APIResponse<string> { StatusCode = 200, Message = "Account deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string> { StatusCode = 400, Message = ex.Message });
            }
        }

        [HttpPut("reset-password/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromRoute] short id, [FromForm] string curPass, [FromForm] string newPass)
        {
            try
            {
                var reseted = await _service.UpdatePasswordAsync(id, curPass, newPass);

                return Ok(new APIResponse<string> { StatusCode = 200, Message = "Change password successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse<string> { StatusCode = 400, Message = ex.Message });
            }
        }
    }
}
