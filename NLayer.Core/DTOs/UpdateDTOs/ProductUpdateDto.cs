namespace NLayer.Core.DTOs.UpdateDTOs
{
    public class ProductUpdateDto : UpdateBaseDto
    {
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
