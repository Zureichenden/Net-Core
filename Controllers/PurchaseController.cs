using System.Text.Json;
using System.Text.Json.Serialization;
using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchasesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PurchasesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePurchase([FromBody] PurchaseDto purchaseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Si el modelo no es válido, devolver el error
            }

            var purchase = new Purchase
            {
                UserId = purchaseDto.UserId,
                PurchaseDate = DateTime.UtcNow,
                TotalAmount = purchaseDto.PurchaseDetails.Sum(pd => pd.UnitPrice * pd.Quantity),
                PurchaseDetails = purchaseDto.PurchaseDetails.Select(pd => new PurchaseDetail
                {
                    ArticleId = pd.ArticleId,
                    Quantity = pd.Quantity,
                    UnitPrice = pd.UnitPrice
                }).ToList()
            };

            // Guardar la compra en la base de datos
            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            // Configurar opciones de serialización para evitar ciclos
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            // Devolver la respuesta serializada
            var response = JsonSerializer.Serialize(purchase, options);
            return Ok(response);  // Devolver la respuesta de la compra creada
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPurchasesByUser(int userId)
        {
            var purchases = await _context.Purchases
                .Include(p => p.PurchaseDetails)
                .ThenInclude(pd => pd.Article)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return Ok(purchases);
        }
    }
}
