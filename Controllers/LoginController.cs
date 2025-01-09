using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Models.DTOs; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/auth")] 
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username || u.Email == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or email.");
            }

            if (user.Password != loginDto.Password)
            {
                return Unauthorized("Invalid password.");
            }

            return Ok(new { message = "Login successful", user });
        }
    }
}
