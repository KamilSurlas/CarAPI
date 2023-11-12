using CarAPI.Entities;
using FluentValidation;

namespace CarAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly CarDbContext _context;
        public RegisterUserDtoValidator(CarDbContext context)
        {
            _context = context;

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).Equal(u => u.Password);
            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var existingEmail = _context.Users.Any(u => u.Email == value);
                if(existingEmail)
                {
                    context.AddFailure("Email", $"Email {value} is already in use");
                }
            });
        }
    }
}
