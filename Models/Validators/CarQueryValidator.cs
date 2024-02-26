using CarAPI.Entities;
using FluentValidation;

namespace CarAPI.Models.Validators
{
    public class CarQueryValidator : AbstractValidator<Query>
    {
        private int[] allowedPageSizes = { 5, 10, 15 };
        private string[] allowedSortByNames = { nameof(Car.BrandName), nameof(Car.ModelName), nameof(Car.ProductionYear), nameof(Car.Mileage), nameof(Car.RegistrationNumber) };
        public CarQueryValidator()
        {
            RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(q => q.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
            RuleFor(q => q.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByNames.Contains(value))
                .WithMessage($"Sort by is optional or must be in [{string.Join(", ", allowedSortByNames)}]");
        }
    }
}
