using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models
{
    public class User
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        // Nombre completo de la persona
        public string Name { get; set; }

        // Nombre de usuario
        public string Username { get; set; }

        // Email de la persona
        public string Email { get; set; }

        // Contraseña de la persona (sería buena idea encriptarla antes de guardarla)
        public string Password { get; set; }
    }
}
