using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/account")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
    }
}
