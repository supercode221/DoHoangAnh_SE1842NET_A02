using FUNewsManagement_CoreAPI.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ODataController
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }
    }
}
