using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Models.DTOs; // Importar LoginDto
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/auth")] // Rutas específicas para autenticación
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // Validación del login de un usuario
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login data.");
            }

            // Buscar al usuario por el nombre de usuario o correo electrónico
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username || u.Email == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or email.");
            }

            // Validar la contraseña (en una aplicación real, se debe comparar con la contraseña encriptada)
            if (user.Password != loginDto.Password)
            {
                return Unauthorized("Invalid password.");
            }

            return Ok(new { message = "Login successful", user });
        }
    }
}
