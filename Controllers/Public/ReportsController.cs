using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Craftmatrix.org.Model;

namespace SiglatCoreAPI.Controllers.Public
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    public class ReportsController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<PagedResponse<ReportDto>> GetReports([FromQuery] PaginationParams pagination)
        {
            // Mock data for demonstration
            var mockReports = new List<ReportDto>
            {
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Fire Emergency",
                    Description = "House fire reported on Main Street",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    InvolvedAgencies = "Fire Department, Police",
                    Notes = "Multiple units dispatched",
                    ReporterName = "John Doe",
                    ReporterEmail = "john.doe@example.com"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Medical Emergency",
                    Description = "Heart attack victim at downtown plaza",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-1),
                    InvolvedAgencies = "Ambulance, Hospital",
                    Notes = "Patient transported to General Hospital",
                    ReporterName = "Jane Smith",
                    ReporterEmail = "jane.smith@example.com"
                },
                new ReportDto 
                { 
                    Id = Guid.NewGuid(),
                    IncidentType = "Traffic Accident",
                    Description = "Multi-vehicle collision on Highway 101",
                    WhoReportedId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddMinutes(-30),
                    InvolvedAgencies = "Police, Ambulance, Tow Service",
                    Notes = "Road blocked, traffic diverted",
                    ReporterName = "Mike Johnson",
                    ReporterEmail = "mike.johnson@example.com"
                }
            };

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                mockReports = mockReports.Where(r => 
                    r.IncidentType.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    (r.ReporterName?.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(pagination.SortBy))
            {
                mockReports = pagination.SortBy.ToLower() switch
                {
                    "timestamp" => pagination.SortOrder?.ToLower() == "desc" 
                        ? mockReports.OrderByDescending(r => r.Timestamp).ToList()
                        : mockReports.OrderBy(r => r.Timestamp).ToList(),
                    "incidenttype" => pagination.SortOrder?.ToLower() == "desc"
                        ? mockReports.OrderByDescending(r => r.IncidentType).ToList()
                        : mockReports.OrderBy(r => r.IncidentType).ToList(),
                    _ => mockReports.OrderByDescending(r => r.Timestamp).ToList()
                };
            }
            else
            {
                mockReports = mockReports.OrderByDescending(r => r.Timestamp).ToList();
            }

            var totalCount = mockReports.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);
            
            var pagedData = mockReports
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            
            var response = new PagedResponse<ReportDto>
            {
                Data = pagedData,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                NextPageUrl = pagination.Page < totalPages 
                    ? $"{baseUrl}?page={pagination.Page + 1}&pageSize={pagination.PageSize}" 
                    : null,
                PreviousPageUrl = pagination.Page > 1 
                    ? $"{baseUrl}?page={pagination.Page - 1}&pageSize={pagination.PageSize}" 
                    : null
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ReportDto> GetReport(Guid id)
        {
            var mockReport = new ReportDto 
            { 
                Id = id,
                IncidentType = "Fire Emergency",
                Description = "House fire reported on Main Street",
                WhoReportedId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddHours(-2),
                InvolvedAgencies = "Fire Department, Police",
                Notes = "Multiple units dispatched",
                ReporterName = "John Doe",
                ReporterEmail = "john.doe@example.com"
            };

            return Ok(mockReport);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<ReportDto> CreateReport([FromBody] ReportDto report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            report.Id = Guid.NewGuid();
            report.CreatedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;

            return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public ActionResult<ReportDto> UpdateReport(Guid id, [FromBody] ReportDto report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            report.Id = id;
            report.UpdatedAt = DateTime.UtcNow;

            return Ok(report);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult DeleteReport(Guid id)
        {
            return NoContent();
        }
    }
}