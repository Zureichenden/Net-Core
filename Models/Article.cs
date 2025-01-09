using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models
{
    public class Article
    {
        // Constantes para los estados
        public const int ENABLED = 1;
        public const int DISABLED = 2;
        public const int DELETED = 99;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ImageUrl { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "Status must be 1 (enabled), 2 (disabled), or 99 (deleted).")]
        public int Status { get; set; } = ENABLED; // Default is enabled
    }
}
