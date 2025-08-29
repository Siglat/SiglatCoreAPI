using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Craftmatrix.org.Model;
using System.ComponentModel.DataAnnotations;

namespace SiglatCoreAPI.Controllers.Public
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/[controller]")]
    public class AuthController : ControllerBase
    {
        public class LoginRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            
            [Required]
            [MinLength(6)]
            public string Password { get; set; } = string.Empty;
        }

        public class RegisterRequest
        {
            [Required]
            [StringLength(50)]
            public string FirstName { get; set; } = string.Empty;
            
            [Required]
            [StringLength(50)]
            public string LastName { get; set; } = string.Empty;
            
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            
            [Required]
            [MinLength(6)]
            public string Password { get; set; } = string.Empty;
            
            [Phone]
            public string? PhoneNumber { get; set; }
            
            public string? Department { get; set; }
            public string? Location { get; set; }
        }

        public class AuthResponse
        {
            public string Token { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
            public ProfileDto User { get; set; } = new();
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(new { message = "Public Auth API is healthy", timestamp = DateTime.UtcNow });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mock registration - in real implementation, validate email doesn't exist, hash password, save to DB
            var user = new ProfileDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                Location = request.Location
            };

            var response = new AuthResponse
            {
                Token = GenerateMockJwtToken(),
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = user
            };

            return CreatedAtAction(nameof(Health), response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mock login validation - in real implementation, verify email/password against DB
            if (request.Email == "test@example.com" && request.Password == "password123")
            {
                var user = new ProfileDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = request.Email,
                    PhoneNumber = "+1-555-0123",
                    Department = "Emergency Services",
                    Location = "Main Office"
                };

                var response = new AuthResponse
                {
                    Token = GenerateMockJwtToken(),
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = user
                };

                return Ok(response);
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> RefreshToken([FromBody] string token)
        {
            // Mock token refresh - in real implementation, validate refresh token
            var response = new AuthResponse
            {
                Token = GenerateMockJwtToken(),
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new ProfileDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test@example.com",
                    Department = "Emergency Services"
                }
            };

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Mock logout - in real implementation, invalidate token
            return Ok(new { message = "Successfully logged out" });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            // Mock forgot password - in real implementation, send reset email
            return Ok(new { message = "Password reset instructions sent to email" });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromBody] object resetData)
        {
            // Mock password reset - in real implementation, validate token and update password
            return Ok(new { message = "Password successfully reset" });
        }

        private string GenerateMockJwtToken()
        {
            // Mock JWT token - in real implementation, generate proper JWT
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(
                $"mock-jwt-token-{DateTime.UtcNow.Ticks}"));
        }
    }
}