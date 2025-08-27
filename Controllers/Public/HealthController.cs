using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Craftmatrix.org.Model;

namespace SiglatCoreAPI.Controllers.Public
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetHealth()
        {
            var health = new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                uptime = Environment.TickCount64 / 1000.0, // seconds
                endpoints = new
                {
                    reports = "/api/v1/public/reports",
                    users = "/api/v1/public/users",
                    auth = "/api/v1/public/auth"
                }
            };

            return Ok(health);
        }

        [HttpGet("detailed")]
        [AllowAnonymous]
        public IActionResult GetDetailedHealth()
        {
            var detailedHealth = new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                uptime = Environment.TickCount64 / 1000.0,
                system = new
                {
                    platform = Environment.OSVersion.Platform.ToString(),
                    version = Environment.OSVersion.VersionString,
                    processorCount = Environment.ProcessorCount,
                    workingSet = Environment.WorkingSet,
                    gcMemory = GC.GetTotalMemory(false)
                },
                database = new
                {
                    status = "connected", // Mock status
                    provider = "PostgreSQL",
                    lastConnection = DateTime.UtcNow.AddMinutes(-1)
                },
                services = new
                {
                    authentication = "operational",
                    authorization = "operational",
                    swagger = "operational"
                }
            };

            return Ok(detailedHealth);
        }
    }
}