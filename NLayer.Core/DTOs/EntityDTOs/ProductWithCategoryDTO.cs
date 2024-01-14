namespace NLayer.Core.DTOs.EntityDTOs
{
    public class ProductWithCategoryDTO : ProductDto 
    {
        public CategoryDto Category { get; set; }
    }
}
