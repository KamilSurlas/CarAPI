using FluentValidation;

namespace CarAPI.Models.Validators
{
    public class CarQueryValidator : AbstractValidator<Query>
    {
        private int[] allowedPageSizes = { 5, 10, 15 };
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
        }
    }
}
