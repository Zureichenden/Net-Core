namespace BackendApi.Models.DTOs
{
    public class ArticleDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
    }
}
