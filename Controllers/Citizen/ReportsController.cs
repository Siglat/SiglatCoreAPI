using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SiglatCoreAPI.Controllers.Citizen
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Citizen")]
    [Route("api/v{version:apiVersion}/citizen/[controller]")]
    public class ReportsController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { message = "Citizen Reports API is healthy", timestamp = DateTime.UtcNow });
        }

        [HttpGet]
        public IActionResult GetReports()
        {
            return Ok(new { message = "Get citizen reports - to be implemented" });
        }

        [HttpPost]
        public IActionResult CreateReport()
        {
            return Ok(new { message = "Create report - to be implemented" });
        }

        [HttpGet("{id}")]
        public IActionResult GetReport(int id)
        {
            return Ok(new { message = $"Get report {id} - to be implemented" });
        }
    }
}