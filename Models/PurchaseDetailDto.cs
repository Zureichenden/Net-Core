namespace BackendApi.Models.DTOs
{
    public class PurchaseDetailDto
    {
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
