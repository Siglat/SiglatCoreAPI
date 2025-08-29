using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Craftmatrix.org.Model;

namespace SiglatCoreAPI.Controllers.PNP
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "PNP")]
    [Route("api/v{version:apiVersion}/pnp/[controller]")]
    public class PoliceOperationsController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { message = "PNP Police Operations API is healthy", timestamp = DateTime.UtcNow });
        }

        [HttpGet("incidents")]
        public ActionResult<PagedResponse<ReportDto>> GetPoliceIncidents([FromQuery] PaginationParams pagination)
        {
            // Mock police-related incidents for PNP
            var mockPoliceIncidents = new List<ReportDto>
            {
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Traffic Accident",
                    Description = "Multi-vehicle collision on Commonwealth Avenue",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    InvolvedAgencies = "PNP-HPG, MMDA, Medical Response",
                    Notes = "3 vehicles involved, 2 injuries, traffic rerouted",
                    ReporterName = "PNP Station 4",
                    ReporterEmail = "station4@pnp.gov.ph"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Robbery",
                    Description = "Armed robbery at convenience store on Taft Avenue",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-1),
                    InvolvedAgencies = "PNP Criminal Investigation, SOCO",
                    Notes = "Suspect fled on motorcycle, CCTV being reviewed",
                    ReporterName = "PNP Malate",
                    ReporterEmail = "malate@pnp.gov.ph"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Public Disturbance",
                    Description = "Large gathering blocking road in Mendiola",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddMinutes(-30),
                    InvolvedAgencies = "PNP Civil Disturbance Management, Manila Police",
                    Notes = "Peaceful assembly, crowd control measures implemented",
                    ReporterName = "PNP Manila",
                    ReporterEmail = "manila@pnp.gov.ph"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Drug Operation",
                    Description = "Anti-drug operation in Barangay 123",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddMinutes(-15),
                    InvolvedAgencies = "PNP-PDEA, Barangay Officials",
                    Notes = "Operation ongoing, 3 suspects arrested",
                    ReporterName = "PNP Anti-Drug Unit",
                    ReporterEmail = "antidrug@pnp.gov.ph"
                }
            };

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                mockPoliceIncidents = mockPoliceIncidents.Where(r => 
                    r.IncidentType.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    (r.Notes?.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            var totalCount = mockPoliceIncidents.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);
            
            var pagedData = mockPoliceIncidents
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
        public IActionResult RespondToIncident(Guid incidentId, [FromBody] object responseData)
        {
            return Ok(new { 
                message = $"PNP response logged for incident {incidentId}", 
                timestamp = DateTime.UtcNow,
                status = "units_dispatched"
            });
        }

        [HttpGet("units")]
        public IActionResult GetPoliceUnits()
        {
            var mockUnits = new[]
            {
                new { id = 1, name = "Mobile Patrol 1", location = "Makati", status = "available", officers = 2, type = "patrol" },
                new { id = 2, name = "Mobile Patrol 7", location = "Quezon City", status = "responding", officers = 2, type = "patrol" },
                new { id = 3, name = "SWAT Alpha", location = "Camp Crame", status = "standby", officers = 8, type = "special_ops" },
                new { id = 4, name = "Traffic Unit 3", location = "EDSA", status = "active", officers = 4, type = "traffic" },
                new { id = 5, name = "K-9 Unit", location = "Manila", status = "available", officers = 3, type = "k9" },
                new { id = 6, name = "Motorcycle Unit", location = "BGC", status = "patrol", officers = 1, type = "motorcycle" }
            };

            return Ok(new { units = mockUnits, timestamp = DateTime.UtcNow });
        }

        [HttpPost("units/{unitId}/dispatch")]
        public IActionResult DispatchUnit(int unitId, [FromBody] object dispatchData)
        {
            return Ok(new { 
                message = $"Police unit {unitId} dispatched successfully", 
                timestamp = DateTime.UtcNow,
                estimatedArrival = DateTime.UtcNow.AddMinutes(12)
            });
        }

        [HttpGet("patrols")]
        public IActionResult GetActivePatrols()
        {
            var activePatrols = new[]
            {
                new { 
                    id = 1, 
                    callSign = "Charlie-7", 
                    location = "Ayala Avenue", 
                    officers = new[] { "PO1 Santos", "PO2 Cruz" },
                    startTime = DateTime.UtcNow.AddHours(-4),
                    status = "active"
                },
                new { 
                    id = 2, 
                    callSign = "Delta-3", 
                    location = "Ortigas Center", 
                    officers = new[] { "PO3 Garcia", "PO1 Reyes" },
                    startTime = DateTime.UtcNow.AddHours(-6),
                    status = "break"
                },
                new { 
                    id = 3, 
                    callSign = "Echo-9", 
                    location = "Bonifacio Global City", 
                    officers = new[] { "SPO1 Mendoza" },
                    startTime = DateTime.UtcNow.AddHours(-2),
                    status = "responding"
                }
            };

            return Ok(new { patrols = activePatrols, timestamp = DateTime.UtcNow });
        }

        [HttpGet("statistics")]
        public IActionResult GetPoliceStatistics()
        {
            var stats = new
            {
                today = new
                {
                    incidents = 28,
                    resolved = 22,
                    ongoing = 6,
                    averageResponseTime = "12.3 minutes",
                    arrests = 8
                },
                thisMonth = new
                {
                    incidents = 642,
                    resolved = 598,
                    ongoing = 44,
                    averageResponseTime = "11.8 minutes",
                    arrests = 156
                },
                incidentTypes = new[]
                {
                    new { type = "Traffic Violations", count = 245 },
                    new { type = "Theft/Robbery", count = 89 },
                    new { type = "Public Disturbance", count = 76 },
                    new { type = "Drug-related", count = 45 },
                    new { type = "Domestic Violence", count = 34 },
                    new { type = "Assault", count = 28 }
                },
                criminalInvestigation = new
                {
                    openCases = 156,
                    solvedCases = 89,
                    solveRate = "57.1%"
                }
            };

            return Ok(stats);
        }

        [HttpPost("alerts")]
        public IActionResult CreatePoliceAlert([FromBody] object alertData)
        {
            return Ok(new { 
                message = "Police alert created and dispatched to units", 
                alertId = Guid.NewGuid(),
                timestamp = DateTime.UtcNow,
                priority = "high"
            });
        }

        [HttpGet("warrants")]
        public IActionResult GetActiveWarrants([FromQuery] PaginationParams pagination)
        {
            var mockWarrants = new[]
            {
                new { 
                    id = Guid.NewGuid(),
                    suspectName = "Juan Dela Cruz",
                    charges = "Theft, Estafa",
                    issueDate = DateTime.UtcNow.AddDays(-15),
                    status = "active",
                    lastKnownLocation = "Tondo, Manila"
                },
                new { 
                    id = Guid.NewGuid(),
                    suspectName = "Maria Santos",
                    charges = "Drug Trafficking",
                    issueDate = DateTime.UtcNow.AddDays(-8),
                    status = "active",
                    lastKnownLocation = "Quezon City"
                }
            };

            return Ok(new { warrants = mockWarrants, totalCount = mockWarrants.Length });
        }

        [HttpPost("incidents/{incidentId}/evidence")]
        public IActionResult SubmitEvidence(Guid incidentId, [FromBody] object evidenceData)
        {
            return Ok(new { 
                message = $"Evidence submitted for incident {incidentId}", 
                evidenceId = Guid.NewGuid(),
                timestamp = DateTime.UtcNow,
                status = "under_review"
            });
        }
    }
}