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
        public async Task<IActionResult> GetArticles([FromQuery] string? name, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int? stock)
        {
            var query = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Contains(name));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(a => a.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(a => a.Price <= maxPrice.Value);
            }

            if (stock.HasValue)
            {
                query = query.Where(a => a.Stock >= stock.Value);
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
    }
}
