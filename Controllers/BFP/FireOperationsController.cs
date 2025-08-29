using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Craftmatrix.org.Model;

namespace SiglatCoreAPI.Controllers.BFP
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "BFP")]
    [Route("api/v{version:apiVersion}/bfp/[controller]")]
    public class FireOperationsController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { message = "BFP Fire Operations API is healthy", timestamp = DateTime.UtcNow });
        }

        [HttpGet("incidents")]
        public ActionResult<PagedResponse<ReportDto>> GetFireIncidents([FromQuery] PaginationParams pagination)
        {
            // Mock fire-related incidents for BFP
            var mockFireIncidents = new List<ReportDto>
            {
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Structure Fire",
                    Description = "Residential house fire on Rizal Avenue",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-3),
                    InvolvedAgencies = "BFP District 1, BFP Rescue",
                    Notes = "3-alarm fire, evacuation complete",
                    ReporterName = "Fire Station 12",
                    ReporterEmail = "station12@bfp.gov.ph"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Vehicle Fire",
                    Description = "Bus fire on EDSA highway",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-1),
                    InvolvedAgencies = "BFP Highway Patrol, Traffic Management",
                    Notes = "Highway blocked, no casualties",
                    ReporterName = "Fire Station 25",
                    ReporterEmail = "station25@bfp.gov.ph"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Forest Fire",
                    Description = "Wildfire in Sierra Madre mountains",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddMinutes(-45),
                    InvolvedAgencies = "BFP Forest Unit, DENR, Local Volunteers",
                    Notes = "Aerial support requested",
                    ReporterName = "Forest Fire Team",
                    ReporterEmail = "forest@bfp.gov.ph"
                }
            };

            // Apply fire-specific filters
            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                mockFireIncidents = mockFireIncidents.Where(r => 
                    r.IncidentType.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    (r.Notes?.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            var totalCount = mockFireIncidents.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);
            
            var pagedData = mockFireIncidents
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var response = new PagedResponse<ReportDto>
            {
                Data = pagedData,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        [HttpPost("incidents/{incidentId}/respond")]
        public IActionResult RespondToFireIncident(Guid incidentId, [FromBody] object responseData)
        {
            return Ok(new { 
                message = $"BFP response logged for incident {incidentId}", 
                timestamp = DateTime.UtcNow,
                status = "responding"
            });
        }

        [HttpGet("units")]
        public IActionResult GetFireUnits()
        {
            var mockUnits = new[]
            {
                new { id = 1, name = "Fire Station 1", location = "Manila Central", status = "available", personnel = 8 },
                new { id = 2, name = "Fire Station 12", location = "Quezon City", status = "responding", personnel = 6 },
                new { id = 3, name = "Rescue Unit Alpha", location = "Makati", status = "available", personnel = 4 },
                new { id = 4, name = "Ladder Truck 5", location = "Pasig", status = "maintenance", personnel = 5 }
            };

            return Ok(new { units = mockUnits, timestamp = DateTime.UtcNow });
        }

        [HttpPost("units/{unitId}/dispatch")]
        public IActionResult DispatchUnit(int unitId, [FromBody] object dispatchData)
        {
            return Ok(new { 
                message = $"Unit {unitId} dispatched successfully", 
                timestamp = DateTime.UtcNow,
                estimatedArrival = DateTime.UtcNow.AddMinutes(15)
            });
        }

        [HttpGet("equipment")]
        public IActionResult GetEquipmentStatus()
        {
            var mockEquipment = new[]
            {
                new { type = "Fire Trucks", available = 15, total = 18, maintenance = 3 },
                new { type = "Ladder Trucks", available = 8, total = 10, maintenance = 2 },
                new { type = "Rescue Vehicles", available = 12, total = 15, maintenance = 3 },
                new { type = "Water Tankers", available = 6, total = 8, maintenance = 2 }
            };

            return Ok(new { equipment = mockEquipment, lastUpdated = DateTime.UtcNow });
        }

        [HttpGet("statistics")]
        public IActionResult GetFireStatistics()
        {
            var stats = new
            {
                today = new
                {
                    incidents = 12,
                    resolved = 8,
                    ongoing = 4,
                    averageResponseTime = "8.5 minutes"
                },
                thisMonth = new
                {
                    incidents = 287,
                    resolved = 265,
                    ongoing = 22,
                    averageResponseTime = "9.2 minutes"
                },
                incidentTypes = new[]
                {
                    new { type = "Structure Fire", count = 145 },
                    new { type = "Vehicle Fire", count = 68 },
                    new { type = "Grass Fire", count = 42 },
                    new { type = "Electrical Fire", count = 32 }
                }
            };

            return Ok(stats);
        }

        [HttpPost("alerts")]
        public IActionResult CreateFireAlert([FromBody] object alertData)
        {
            return Ok(new { 
                message = "Fire alert created and dispatched to units", 
                alertId = Guid.NewGuid(),
                timestamp = DateTime.UtcNow
            });
        }
    }
}