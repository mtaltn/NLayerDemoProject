namespace NLayer.Core.DTOs.CreateDTOs
{
    public class ProductCreateDto : CreateBaseDto
    {
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
