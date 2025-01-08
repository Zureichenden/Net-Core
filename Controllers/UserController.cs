using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Models.DTOs; // Importar UserDto
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/users")] // Rutas específicas para usuarios
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Registrar un nuevo usuario
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            // Verificar si ya existe un usuario con el mismo username o email
            if (_context.Users.Any(u => u.Username == userDto.Username || u.Email == userDto.Email))
            {
                return BadRequest("Username or Email already exists.");
            }

            // Crear un nuevo usuario a partir de UserDto
            var user = new User
            {
                Name = userDto.Name,
                Username = userDto.Username,  // Asignar el nombre de usuario
                Email = userDto.Email,
                Password = userDto.Password  // Asignar la contraseña (debería encriptarse en una aplicación real)
            };

            // Agregar el nuevo usuario a la base de datos
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully", user });
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // Obtener un usuario por su Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
    }
}
