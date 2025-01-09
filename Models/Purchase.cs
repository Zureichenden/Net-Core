using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Usuario que realizó la compra
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        // Fecha de la compra
        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        // Total de la compra
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        // Relación con los detalles de la compra
        [Required]  // Asegúrate de que este campo sea obligatorio
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }

}
