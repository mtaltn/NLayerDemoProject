using FluentValidation;
using NLayer.Core.DTOs.CreateDTOs;
using NLayer.Core.DTOs.EntityDTOs;

namespace NLayer.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).
                NotNull().WithMessage("{PropertyName} is required").
                NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Price).
                InclusiveBetween(1, decimal.MaxValue).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(x => x.Stock).
                InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(x => x.CategoryId).
                InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
        }
        /*
         * learn more information about fluentvalidation use that url https://docs.fluentvalidation.net/en/latest/built-in-validators.html
         * or if you wanna write a custom validation use that url https://docs.fluentvalidation.net/en/latest/custom-validators.html 
         */
    }
}
