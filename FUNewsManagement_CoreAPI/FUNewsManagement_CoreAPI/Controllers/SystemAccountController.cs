using FUNewsManagement_CoreAPI.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/account")]
    [ApiController]
    public class SystemAccountController : ODataController
    {
        private readonly ISystemAccountService _service;

        public SystemAccountController(ISystemAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllAccount()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetAccountById([FromRoute] short id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditAccountById([FromRoute] short id)
        {
            return Ok();
        }

        [HttpPut("password/reset/{id}")]
        public IActionResult ResetPasswordById([FromRoute] short id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccountById([FromRoute] short id)
        {
            return Ok();
        }
    }
}
