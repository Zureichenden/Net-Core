using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleDto articleDto)
        {
            if (articleDto == null)
            {
                return BadRequest("Invalid article data.");
            }
            Console.WriteLine($"Received Article: Name = {articleDto.Name}, Description = {articleDto.Description}, Price = {articleDto.Price}, Stock = {articleDto.Stock}");

            var article = new Article
            {
                Name = articleDto.Name,
                Description = articleDto.Description,
                Price = articleDto.Price,
                Stock = articleDto.Stock,
                ImageUrl = articleDto.ImageUrl
            };

            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Article created successfully", article });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error creating article: {ex.InnerException?.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] string? name,
                                              [FromQuery] decimal? minPrice,
                                              [FromQuery] decimal? maxPrice,
                                              [FromQuery] int? stock,
                                              [FromQuery] int? status) // Añadido el parámetro status
        {
            var query = _context.Articles.AsQueryable();

            // Filtrar por nombre
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Contains(name));
            }

            // Filtrar por precio mínimo
            if (minPrice.HasValue)
            {
                query = query.Where(a => a.Price >= minPrice.Value);
            }

            // Filtrar por precio máximo
            if (maxPrice.HasValue)
            {
                query = query.Where(a => a.Price <= maxPrice.Value);
            }

            // Filtrar por stock
            if (stock.HasValue)
            {
                query = query.Where(a => a.Stock >= stock.Value);
            }

            // Filtrar por estado
            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            var articles = await query.ToListAsync();

            return Ok(articles);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return Ok(article);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] ArticleDto articleDto)
        {
            if (articleDto == null)
            {
                return BadRequest("Invalid article data.");
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            article.Name = articleDto.Name;
            article.Description = articleDto.Description;
            article.Price = articleDto.Price;
            article.Stock = articleDto.Stock;
            article.ImageUrl = articleDto.ImageUrl;

            try
            {
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Article updated successfully", article });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error updating article: {ex.InnerException?.Message}");
            }
        }


        // Método para habilitar un artículo (Status = 1)
        [HttpPatch("enable/{id}")]
        public async Task<IActionResult> EnableArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            article.Status = 1;  // Habilitado
            try
            {
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Article enabled successfully", article });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error enabling article: {ex.InnerException?.Message}");
            }
        }

        // Método para deshabilitar un artículo (Status = 2)
        [HttpPatch("disable/{id}")]
        public async Task<IActionResult> DisableArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            article.Status = 2;  // Deshabilitado
            try
            {
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Article disabled successfully", article });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error disabling article: {ex.InnerException?.Message}");
            }
        }

        // Método para eliminar un artículo (Status = 99)
        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            article.Status = 99;  // Eliminado
            try
            {
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Article deleted successfully", article });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error deleting article: {ex.InnerException?.Message}");
            }
        }

    }


}
