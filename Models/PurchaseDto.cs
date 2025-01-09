using System.Collections.Generic;

namespace BackendApi.Models.DTOs
{
    public class PurchaseDto
    {
        public int UserId { get; set; }
        public List<PurchaseDetailDto> PurchaseDetails { get; set; }
    }
}
