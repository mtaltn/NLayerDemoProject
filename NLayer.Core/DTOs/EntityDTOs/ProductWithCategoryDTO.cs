namespace NLayer.Core.DTOs.EntityDTOs
{
    public class ProductWithCategoryDto : ProductDto 
    {
        public CategoryDto Category { get; set; }
    }
}
