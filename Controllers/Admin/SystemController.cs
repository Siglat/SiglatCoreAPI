using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SiglatCoreAPI.Controllers.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,BFP,PNP")]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class SystemController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { message = "Admin System API is healthy", timestamp = DateTime.UtcNow });
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(new { message = "Get all users - to be implemented" });
        }

        [HttpPost("users")]
        public IActionResult CreateUser()
        {
            return Ok(new { message = "Create user - to be implemented" });
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            return Ok(new { message = "Get system statistics - to be implemented" });
        }
    }
}