using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Craftmatrix.org.Model;

namespace SiglatCoreAPI.Controllers.Public
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<PagedResponse<ProfileDto>> GetUsers([FromQuery] PaginationParams pagination)
        {
            // Mock data for demonstration
            var mockUsers = new List<ProfileDto>
            {
                new ProfileDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "+1-555-0123",
                    Department = "Emergency Services",
                    Location = "Downtown Station"
                },
                new ProfileDto
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "+1-555-0124",
                    Department = "Medical Services",
                    Location = "General Hospital"
                },
                new ProfileDto
                {
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Email = "mike.johnson@example.com",
                    PhoneNumber = "+1-555-0125",
                    Department = "Fire Department",
                    Location = "Central Fire Station"
                },
                new ProfileDto
                {
                    FirstName = "Sarah",
                    LastName = "Wilson",
                    Email = "sarah.wilson@example.com",
                    PhoneNumber = "+1-555-0126",
                    Department = "Police Department",
                    Location = "North Precinct"
                },
                new ProfileDto
                {
                    FirstName = "David",
                    LastName = "Brown",
                    Email = "david.brown@example.com",
                    PhoneNumber = "+1-555-0127",
                    Department = "Ambulance Services",
                    Location = "East Station"
                }
            };

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                mockUsers = mockUsers.Where(u => 
                    u.FirstName.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ||
                    (u.Department?.Contains(pagination.Search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(pagination.SortBy))
            {
                mockUsers = pagination.SortBy.ToLower() switch
                {
                    "firstname" => pagination.SortOrder?.ToLower() == "desc" 
                        ? mockUsers.OrderByDescending(u => u.FirstName).ToList()
                        : mockUsers.OrderBy(u => u.FirstName).ToList(),
                    "lastname" => pagination.SortOrder?.ToLower() == "desc"
                        ? mockUsers.OrderByDescending(u => u.LastName).ToList()
                        : mockUsers.OrderBy(u => u.LastName).ToList(),
                    "email" => pagination.SortOrder?.ToLower() == "desc"
                        ? mockUsers.OrderByDescending(u => u.Email).ToList()
                        : mockUsers.OrderBy(u => u.Email).ToList(),
                    "department" => pagination.SortOrder?.ToLower() == "desc"
                        ? mockUsers.OrderByDescending(u => u.Department).ToList()
                        : mockUsers.OrderBy(u => u.Department).ToList(),
                    _ => mockUsers.OrderBy(u => u.FirstName).ToList()
                };
            }
            else
            {
                mockUsers = mockUsers.OrderBy(u => u.FirstName).ToList();
            }

            var totalCount = mockUsers.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);
            
            var pagedData = mockUsers
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            
            var response = new PagedResponse<ProfileDto>
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

        [HttpGet("{email}")]
        [AllowAnonymous]
        public ActionResult<ProfileDto> GetUser(string email)
        {
            var mockUser = new ProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                PhoneNumber = "+1-555-0123",
                Department = "Emergency Services",
                Location = "Downtown Station"
            };

            return Ok(mockUser);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<ProfileDto> CreateUser([FromBody] ProfileDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(GetUser), new { email = user.Email }, user);
        }

        [HttpPut("{email}")]
        [AllowAnonymous]
        public ActionResult<ProfileDto> UpdateUser(string email, [FromBody] ProfileDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Email = email;
            return Ok(user);
        }

        [HttpDelete("{email}")]
        [AllowAnonymous]
        public IActionResult DeleteUser(string email)
        {
            return NoContent();
        }
    }
}