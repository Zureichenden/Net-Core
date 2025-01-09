using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendApi.Models
{
    public class PurchaseDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Compra asociada
        [Required]
        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }

        [JsonIgnore]  // Ignorar la referencia de vuelta a Purchase al serializar
        public Purchase Purchase { get; set; }

        // Artículo asociado
        [Required]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        // Cantidad comprada
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Precio unitario en el momento de la compra
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
    }
}
