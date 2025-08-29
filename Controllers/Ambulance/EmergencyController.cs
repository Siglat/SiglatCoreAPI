using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SiglatCoreAPI.Controllers.Ambulance
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Ambulance,BFP,PNP")]
    [Route("api/v{version:apiVersion}/ambulance/[controller]")]
    public class EmergencyController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { message = "Ambulance Emergency API is healthy", timestamp = DateTime.UtcNow });
        }

        [HttpGet("alerts")]
        public IActionResult GetAlerts()
        {
            return Ok(new { message = "Get emergency alerts - to be implemented" });
        }

        [HttpPost("respond/{alertId}")]
        public IActionResult RespondToAlert(int alertId)
        {
            return Ok(new { message = $"Respond to alert {alertId} - to be implemented" });
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { message = "Get ambulance status - to be implemented" });
        }
    }
}